namespace WebUser.PricingService;

using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.Promotion.PromoBuilder.Actions;
using WebUser.features.Promotion.PromoBuilder.Condition;
using WebUser.PricingService.DTO;
using WebUser.PricingService.interfaces;

public class PricingService : IPricingService
{
    private readonly DB_Context dbcontext;

    public PricingService(DB_Context context)
    {
        dbcontext = context;
    }

    /// <summary>
    /// apllies active discounts to the products
    /// </summary>
    /// <param name="productIds"></param>
    /// <returns>list of discount values</returns>
    public async Task<List<DiscountRecordDTO>> ApplyDiscountAsync(ICollection<int> productIds)
    {
        var products = await dbcontext.Products.Include(q => q.Discounts).Where(q => productIds.Contains(q.ID)).ToListAsync();
        var discountRecords = new List<DiscountRecordDTO>();
        foreach (var product in products)
        {
            var productActiveDiscounts = product.Discounts.Where(discount => Discount.IsActive(discount)).ToList();
            var basePrice = product.Price;

            productActiveDiscounts.ForEach(discount =>
            {
                var record = new DiscountRecordDTO { ProductId = product.ID, Type = DiscountType.ProductDiscount, ValueTypes = new List<DiscountValueType>() };
                if (discount.DiscountPercent.HasValue && discount.DiscountPercent > 0)
                {
                    record.PercentDiscountValue = Math.Floor(basePrice * discount.DiscountPercent.Value / 100);
                    record.Percent = discount.DiscountPercent;
                    record.ValueTypes.Add(DiscountValueType.Percentage);
                }
                if (discount.DiscountVal.HasValue && discount.DiscountVal > 0)
                {
                    record.AbsoluteDiscountValue = discount.DiscountVal;
                    record.ValueTypes.Add(DiscountValueType.Absolute);
                }
                discountRecords.Add(record);
            });
        }

        return discountRecords;
    }

    /// <summary>
    /// apllies entered coupons to the cart items
    /// </summary>
    /// <param name="couponsCodes"></param>
    /// <param name="productIds"></param>
    /// <returns>list of items and their discount value and list of activated coupons</returns>
    public async Task<List<(DiscountRecordDTO record, Coupon coupon)>> ApplyCouponsAsync(ICollection<int> productIds, string couponsCodes)
    {
        char[] splitChars = { ' ', ',', ';', '-', '_', '.', ':', '\t' };
        List<string> inputCodes = couponsCodes.ToLower().Split(splitChars).ToList();
        var activatedCoupons = new List<(DiscountRecordDTO record, Coupon coupon)>();

        var coupons = await dbcontext
        .Coupons.Where(q => inputCodes.Contains(q.Code.ToLower()) && productIds.Contains(q.ProductID))
        .ToListAsync();

        coupons = coupons.Where(Coupon.IsActive).ToList();
        var products = await dbcontext.Products.Include(q => q.Coupons).Where(q => productIds.Contains(q.ID)).ToListAsync();
        foreach (var coupon in coupons)
        {
            foreach (var product in products.Where(pr => pr.ID == coupon.ProductID))//coupon applies only to a one product exclusevely
            {

                var record = new DiscountRecordDTO { ProductId = product.ID, Type = DiscountType.CouponDiscount, ValueTypes = new List<DiscountValueType>() };
                var basePrice = coupon.Product.Price;
                if (coupon.DiscountPercent.HasValue && coupon.DiscountPercent > 0)
                {
                    record.PercentDiscountValue = Math.Floor(basePrice * coupon.DiscountPercent.Value / 100);
                    record.Percent = coupon.DiscountPercent;
                    record.ValueTypes.Add(DiscountValueType.Percentage);
                }
                if (coupon.DiscountVal.HasValue && coupon.DiscountVal > 0)
                {
                    record.AbsoluteDiscountValue = coupon.DiscountVal;
                    record.ValueTypes.Add(DiscountValueType.Absolute);
                }
                coupon.IsActivated = true;
                activatedCoupons.Add((record, coupon));
            }


        }
        return activatedCoupons;
    }

    /// <summary>
    /// apllies user's availible points to the cart items
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="pointsValue"></param>
    /// <returns>List of used point or empty list if there's not enought points on user balance</returns>
    public async Task<(DiscountRecordDTO discount, List<Point> activatedPoints)> ApplyPointsAsync(string userId, int pointsValue)
    {

        var points = await dbcontext.Points.Where(q => q.UserID == userId).ToListAsync();
        int pointsUsed = 0;

        List<Point> activatedPoints = new List<Point>();
        int totalPointsAvailable = points.Where(q => Point.IsActive(q)).Sum(w => w.Value);

        var record = new DiscountRecordDTO { Type = DiscountType.BonusPointsDiscount, ValueTypes = new List<DiscountValueType>(), };
        record.ValueTypes.Add(DiscountValueType.Absolute);
        if (totalPointsAvailable < pointsValue)
        {
            record.AbsoluteDiscountValue = 0;
            return (record, new List<Point>());
        }
        foreach (var point in points)
        {
            if (Point.IsActive(point) && pointsValue > pointsUsed)
            {
                if (point.BalanceLeft > 0)
                {
                    var basepoints = pointsUsed;
                    pointsUsed += Math.Min(point.BalanceLeft, pointsValue - pointsUsed);
                    point.BalanceLeft -= Math.Min(point.BalanceLeft, pointsValue - basepoints);
                    point.IsUsed = true;
                    activatedPoints.Add(point);
                }
            }
            if (pointsValue <= pointsUsed)
            {
                break;
            }
        }
        record.AbsoluteDiscountValue = pointsUsed;
        await dbcontext.SaveChangesAsync();
        return (record, activatedPoints);
    }

    /// <summary>
    /// apllies active promo to the cart items
    /// </summary>
    /// <param name="productCalculations"></param>
    /// <param name="user"></param>
    /// <returns>list of new discounts for products, new point(unbound)</returns>
    public async Task<(List<DiscountRecordDTO> records, Point? newpoint, List<ProductCalculationDTO> freeitems)> ApplyPromosAsync(ICollection<ProductCalculationDTO> productCalculations, User user)
    {
        var promos = await dbcontext
            .Promotions.Include(q => q.AttributeValues)
            .ThenInclude(q => q.AttributeValue)
            .Include(q => q.Categories)
            .ThenInclude(q => q.Category)
            .Include(q => q.ActionProducts)
            .ThenInclude(q => q.Product)
            .Include(q => q.ConditionProducts)
            .ThenInclude(q => q.Product)
            .ToListAsync();

        promos = promos.Where(p => Promotion.IsActive(p)).ToList();

        var products = await GetProductsAsync(dbcontext, productCalculations.Select(q => q.ProductId).ToList());
        Point? point = null;
        var discounts = new List<DiscountRecordDTO>();
        var freeItems = new List<ProductCalculationDTO>();
        var productQuantitiesList = await GetProductQuantityFromProductCalculations(dbcontext,
            productCalculations.Select(q => (id: q.ProductId, quantity: q.Quantity)).ToList());

        foreach (var promotion in promos)
        {
            List<bool> conditionsResult = new List<bool>();

            if (promotion.AttributeValues.Any())
            {
                conditionsResult.Add(products.HasAttributes(promotion.AttributeValues.Select(q => q.AttributeValue)));
            }
            if (promotion.Categories.Any())
            {
                conditionsResult.Add(products.HasCategories(promotion.Categories.Select(q => q.Category)));
            }

            if (promotion.IsFirstOrder ?? false)
            {
                conditionsResult.Add(FirstOrderExtention.IsFirstOrder(user));
            }
            if (promotion.BuyQuantity > 0 && promotion.ConditionProducts.Any())
            {
                conditionsResult.Add(productQuantitiesList.HasQuantityProducts(promotion.ConditionProducts.Select(q => q.Product), promotion.BuyQuantity.Value));
            }
            else if (promotion.ConditionProducts.Any())
            {
                conditionsResult.Add(products.HasProducts(promotion.ConditionProducts.Select(q => q.Product)));
            }
            if (promotion.MinPay > 0)
            {
                conditionsResult.Add(productCalculations.IsPriceBiggerThan(promotion.MinPay.Value));
            }
            if (!conditionsResult.Contains(false))
            {
                if ((promotion.DiscountPercent > 0 || promotion.DiscountVal > 0) && promotion.ActionProducts.Any())
                {
                    discounts.AddRange(
                        products.ToList().GetDiscountForItemtAct(promotion.ActionProducts.Select(q => q.Product),
                        promotion.DiscountVal ?? 0, promotion.DiscountPercent ?? 0)
                    );
                }
                else if (promotion.DiscountPercent > 0 || promotion.DiscountVal > 0)
                {
                    discounts.AddRange(products.ToList().GetDiscountAct(promotion.DiscountVal ?? 0, promotion.DiscountPercent ?? 0));
                }

                if (promotion.PointsPercent > 0 || promotion.PointsValue > 0)
                {
                    point = products.ToList().GetPointsAct(promotion.PointsValue ?? 0, promotion.PointsPercent ?? 0, promotion.PointsExpireDays ?? 0);
                }
                if (promotion.ActionProducts.Any() && promotion.GetQuantity > 0)
                {
                    freeItems.AddRange(products.ToList().GetFreeItemAct(promotion.ActionProducts.Select(q => q.Product), promotion.GetQuantity.Value));
                }
            }
        }
        return (records: discounts, newpoint: point, freeitems: freeItems);
    }

    private static async Task<List<Product>> GetProductsAsync(DB_Context dbcontext, ICollection<int> productIds)
    {
        if (productIds == null || !productIds.Any())
        {
            return new();
        }

        var products = await dbcontext
            .Products.Include(q => q.Coupons)
            .Where(p => productIds.Contains(p.ID))
            .ToListAsync();

        return products;
    }

    private static async Task<List<(Product Product, int Quantity)>> GetProductQuantityFromProductCalculations(DB_Context dbcontext, ICollection<(int id, int quantity)> productIdQuantitys)
    {
        if (productIdQuantitys == null || !productIdQuantitys.Any())
        {
            return new();
        }

        var productIdList = productIdQuantitys.Select(p => p.id).ToList();
        var products = await dbcontext.Products
            .Include(q => q.Coupons) // Include any related entities if needed
            .Where(p => productIdList.Contains(p.ID))
            .ToListAsync();

        var result = products.Select(product => (
            ProductId: product,
            Quantity: productIdQuantitys.FirstOrDefault(p => p.id == product.ID).quantity
        )).ToList();

        return result;
    }

    public async Task<(CartOrderResultDTO order, Point? newPoint, List<Point> activatedPoints, List<Coupon> activatedCoupons)> GenerateOrderAsync(IEnumerable<(int productId, int quantity)> orderProducts, User user, string codes, int pointsRequested)
    {
        //Discount -> Coupon -> Promo -> Points)
        var productIds = orderProducts.Select(p => p.productId).ToList();
        var products = dbcontext.Products.Where(q => productIds.Contains(q.ID)).ToList();
        var discounts = await ApplyDiscountAsync(productIds);
        var couponDiscounts = await ApplyCouponsAsync(productIds, codes);
        var (promoDiscounts, newPoint, freeItems) =
            await ApplyPromosAsync(orderProducts.Select(op => new ProductCalculationDTO
            { ProductId = op.productId, Quantity = op.quantity, BasePrice = products.First(p => p.ID == op.productId).Price }).ToList(), user);


        var (pointsDiscount, activatedPoints) = await ApplyPointsAsync(user.Id, pointsRequested);

        var cartOrderResult = new CartOrderResultDTO();
        cartOrderResult.Products.AddRange(freeItems);
        foreach (var (productId, quantity) in orderProducts)
        {
            var productCalculation = new ProductCalculationDTO
            {
                ProductId = productId,
                Quantity = quantity,
                BasePrice = products.First(p => p.ID == productId).Price,
                Name = products.First(q => q.ID == productId).Name,
                IsPurchasable = Product.IsPurchasable(products.First(q => q.ID == productId), quantity),
                ActivatedCoupons = couponDiscounts.Where(q => q.record.ProductId == productId).Select(q => q.coupon.ID).ToList(),
            };


            productCalculation.AppliedDiscounts.AddRange(discounts.Where(p => p.ProductId == productId)
                .Select(p => new DiscountRecordDTO
                {
                    ProductId = p.ProductId,
                    Type = p.Type,
                    AbsoluteDiscountValue = p.AbsoluteDiscountValue,
                    Percent = p.Percent,
                    PercentDiscountValue = p.PercentDiscountValue,
                    ValueTypes = p.ValueTypes,
                }));
            productCalculation.AppliedDiscounts.AddRange(couponDiscounts.Where(cd => cd.record.ProductId == productId).Select(cd => cd.record));
            productCalculation.AppliedDiscounts.AddRange(promoDiscounts.Where(p => p.ProductId == productId));

            double totalSingleDiscount =
                productCalculation.AppliedDiscounts.Where(d => d.ValueTypes.Contains(DiscountValueType.Absolute)).Sum(d => d.AbsoluteDiscountValue.GetValueOrDefault()) +
                productCalculation.AppliedDiscounts.Where(d => d.ValueTypes.Contains(DiscountValueType.Percentage)).Sum(d => d.PercentDiscountValue.GetValueOrDefault());

            productCalculation.FinalSinglePrice = productCalculation.BasePrice - totalSingleDiscount;
            cartOrderResult.Products.Add(productCalculation);
        }

        if (pointsDiscount != null && pointsDiscount.AbsoluteDiscountValue.HasValue && pointsDiscount.AbsoluteDiscountValue > 0
            && pointsDiscount.Type == DiscountType.BonusPointsDiscount)
        {
            cartOrderResult.UsedBonusPoints = (int)pointsDiscount.AbsoluteDiscountValue.Value;
            cartOrderResult.OrderCost = cartOrderResult.Products.Sum(p => p.FinalSinglePrice * p.Quantity) - pointsDiscount.AbsoluteDiscountValue.Value;
        }
        else
        {
            cartOrderResult.OrderCost = cartOrderResult.Products.Sum(p => p.FinalSinglePrice * p.Quantity);
        }
        var activeCoupons = couponDiscounts.Select(q => q.coupon).ToList();
        return (order: cartOrderResult, newPoint: newPoint, activatedPoints: activatedPoints, activatedCoupons: activeCoupons);
    }

}
