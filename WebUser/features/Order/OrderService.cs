using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Order.Interfaces;
using WebUser.features.Promotion.PromoBuilder.Actions;
using WebUser.features.Promotion.PromoBuilder.Condition;
using E = WebUser.Domain.entities;

namespace WebUser.features.Order
{
    public class OrderService : IOrderService
    {
        private readonly DB_Context dbcontext;

        public OrderService(DB_Context context)
        {
            this.dbcontext = context;
        }

        /// <summary>
        /// apllies active discounts to the cart items
        /// </summary>
        /// <param name="cart"></param>
        /// <returns>list items and their discount value</returns>
        public List<(E.CartItem, double)> ApplyDiscount(IQueryable<E.Cart> cart)
        {
            var itemDiscounts = new List<(E.CartItem, double)>();
            var cartItems = cart.SelectMany(q => q.Items);
            foreach (var item in cartItems)
            {
                var discount = dbcontext.Discounts.Where(q => item.ProductID == q.ProductID && E.Discount.IsActive(q)).FirstOrDefault();
                var basePrice = item.Product.Price;
                double discountVal = 0;
                if (discount.DiscountVal.HasValue && discount.DiscountVal.Value > 0)
                {
                    discountVal += discount.DiscountVal.Value;
                }
                if (discount.DiscountPercent.HasValue && discount.DiscountPercent > 0)
                {
                    discountVal += basePrice * discount.DiscountPercent.Value / 100;
                }
                itemDiscounts.Add((item, discountVal));
            }
            return itemDiscounts;
        }

        /// <summary>
        /// apllies entered coupons to the cart items
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="couponsCodes"></param>
        /// <returns>list of items and their discount value and list of activated coupons</returns>
        public (List<(E.CartItem, double)>, List<(E.CartItem, E.Coupon)>) ApplyCoupons(IQueryable<E.Cart> cart, string couponsCodes)
        {
            char[] splitChars = { ' ', ',', ';', '-', '_', '.', ':', '\t' };
            List<string> inputCodes = couponsCodes.ToLower().Split(splitChars).ToList();
            var activatedCoupons = new List<(E.CartItem, E.Coupon)>();
            var coupons = dbcontext.Coupons.Where(q => inputCodes.Contains(q.Code) && E.Coupon.IsActive(q)).ToList();
            var itemDiscounts = new List<(E.CartItem, double)>();
            var cartItems = cart.SelectMany(q => q.Items);
            foreach (var coupon in coupons)
            {
                foreach (var product in cartItems)
                {
                    if (coupon.ProductID == product.ProductID)
                    {
                        var basePrice = coupon.Product.Price;
                        double discountVal = 0;
                        if (coupon.DiscountVal > 0)
                        {
                            discountVal += coupon.DiscountVal.Value;
                        }
                        if (coupon.DiscountPercent > 0)
                        {
                            discountVal += basePrice * coupon.DiscountPercent.Value / 100;
                        }
                        coupon.IsActivated = true;
                        activatedCoupons.Add((product, coupon));
                        itemDiscounts.Add((product, discountVal));
                    }
                }
            }
            return (itemDiscounts, activatedCoupons);
        }

        /// <summary>
        /// apllies user's availible points to the cart items
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pointsValue"></param>
        /// <returns>List of used point or empty list if there's not enought points on user balance</returns>
        public ICollection<E.Point> ApplyPoints(E.User user, int pointsValue)
        {
            var points = dbcontext.Points.Where(q => q.UserID == user.Id).ToList();
            int pointsUsed = 0;
            List<E.Point> activatedPoints = new List<E.Point>();
            int totalPointsAvailable = points.Where(q => E.Point.IsActive(q)).Sum(w => w.Value);
            if (totalPointsAvailable < pointsValue)
            {
                return activatedPoints;
            }
            foreach (var point in points)
            {
                if (E.Point.IsActive(point) && pointsValue > pointsUsed)
                {
                    if (point.BalanceLeft > 0)
                    {
                        pointsUsed += Math.Min(point.BalanceLeft, pointsValue - pointsUsed);
                        point.BalanceLeft -= Math.Min(point.BalanceLeft, pointsValue - pointsUsed);
                        point.IsUsed = true;
                        activatedPoints.Add(point);
                    }
                }
                if (pointsValue <= pointsUsed)
                {
                    break;
                }
            }

            return activatedPoints;
        }

        /// <summary>
        /// apllies active promo to the cart items
        /// </summary>
        /// <param name="cart"></param>
        /// <returns>list items and their discount value</returns>
        public (List<(E.CartItem, double)>, E.Point) ApplyPromos(IQueryable<E.Cart> cart)
        {
            var promos = dbcontext
                .Promotions.Include(q => q.AttributeValues)
                .ThenInclude(q => q.AttributeValue)
                .Include(q => q.Categories)
                .ThenInclude(q => q.Category)
                .Include(q => q.Products)
                .ThenInclude(q => q.Product)
                .Include(q => q.PromProducts)
                .ThenInclude(q => q.Product)
                .Include(q => q)
                .AsQueryable();
            var point = new E.Point();
            var promoItem = new List<(E.CartItem, double)>();
            List<bool> conditionsResult = new List<bool>();
            foreach (var promotion in promos)
            {
                if (E.Promotion.IsActive(promotion))
                {
                    if (promotion.AttributeValues.Any())
                    {
                        conditionsResult.Add(cart.HasAttributes(promotion.AttributeValues.Select(q => q.AttributeValue)));
                    }
                    if (promotion.Categories.Any())
                    {
                        conditionsResult.Add(cart.HasCategories(promotion.Categories.Select(q => q.Category)));
                    }
                    if (promotion.MinPay > 0)
                    {
                        conditionsResult.Add(cart.IsPriceBiggerThan((int)promotion.MinPay.Value));
                    }
                    if (promotion.IsFirstOrder.Value)
                    {
                        conditionsResult.Add(cart.IsFirstOrder());
                    }
                    if (promotion.BuyQuantity > 0 && promotion.Products.Any())
                    {
                        conditionsResult.Add(cart.HasProducts(promotion.Products.Select(q => q.Product), promotion.BuyQuantity.Value));
                    }
                    else if (promotion.Products.Any())
                    {
                        conditionsResult.Add(cart.HasProducts(promotion.Products.Select(q => q.Product)));
                    }

                    if (!conditionsResult.Contains(false))
                    {
                        if ((promotion.DiscountPercent > 0 || promotion.DiscountVal > 0) && promotion.PromProducts.Any())
                        {
                            promoItem.AddRange(
                                cart.GetDiscountForItemtAct(
                                    promotion.PromProducts.Select(q => q.Product),
                                    promotion.DiscountVal.Value,
                                    promotion.DiscountPercent.Value
                                )
                            );
                        }
                        else if (promotion.DiscountPercent > 0 || promotion.DiscountVal > 0)
                        {
                            promoItem.AddRange(cart.GetDiscountAct(promotion.DiscountVal.Value, promotion.DiscountPercent.Value));
                        }

                        if (promotion.PointsPercent > 0 || promotion.PointsValue > 0)
                        {
                            point = cart.GetPointsAct(promotion.PointsValue.Value, promotion.PointsPercent.Value, promotion.PointsExpireDays.Value);
                        }
                        if (promotion.PromProducts.Any() && promotion.GetQuantity > 0)
                        {
                            promoItem.AddRange(cart.GetFreeItemAct(promotion.PromProducts.Select(q => q.Product), promotion.GetQuantity.Value));
                        }
                    }
                }
            }
            return (promoItem, point);
        }

        public bool IsProcessed(int Id) => dbcontext.Orders.Any(q => q.ID == Id && q.Status == true);
    }
}
