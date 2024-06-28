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

        public List<(E.CartItem, double)> ApplyDiscount(E.Cart cart)
        {
            var itemDiscounts = new List<(E.CartItem, double)>();
            var cartItems = cart.Items;
            foreach (var item in cartItems)
            {
                var discount = dbcontext
                    .Discounts.Where(q => item.ProductID == q.ProductID && q.ActiveFrom <= DateTime.UtcNow && q.ActiveTo >= DateTime.UtcNow)
                    .FirstOrDefault();
                var basePrice = item.Product.Price;
                double discountVal = 0;
                if (discount.DiscountVal > 0)
                {
                    discountVal += discount.DiscountVal;
                }
                if (discount.DiscountPercent > 0)
                {
                    discountVal += basePrice * discount.DiscountPercent / 100;
                }
                itemDiscounts.Add((item, discountVal));
            }
            return itemDiscounts;
        }

        public List<(E.CartItem, double)> ApplyCoupons(E.Cart cart, string couponsCodes)
        {
            char[] splitChars = { ' ', ',', ';', '-', '_', '.', ':', '\t' };
            List<string> inputCodes = couponsCodes.ToLower().Split(splitChars).ToList();
            var coupons = dbcontext.Coupons.Where(q => inputCodes.Contains(q.Code)).ToList();
            var itemDiscounts = new List<(E.CartItem, double)>();
            var cartItems = cart.Items;
            foreach (var coupon in coupons)
            {
                if (coupon.ActiveFrom <= DateTime.UtcNow && coupon.ActiveTo >= DateTime.UtcNow && coupon.IsActivated == false && coupon.Order == null)
                {
                    foreach (var productI in cartItems)
                    {
                        if (coupon.Product.ID == productI.Product.ID)
                        {
                            var basePrice = coupon.Product.Price;
                            double discountVal = 0;
                            if (coupon.DiscountVal > 0)
                            {
                                discountVal += coupon.DiscountVal;
                            }
                            if (coupon.DiscountPercent > 0)
                            {
                                discountVal += basePrice * coupon.DiscountPercent / 100;
                            }
                            coupon.IsActivated = true;
                            var orderProduct = dbcontext.OrderProducts.FirstOrDefault(q => q.Product.ID == productI.Product.ID);
                            orderProduct.ActivatedCoupons.Add(coupon);
                            itemDiscounts.Add((productI, discountVal));
                        }
                    }
                }
            }
            return itemDiscounts;
        }

        public ICollection<E.Point> ApplyPoints(E.User user, int pointsValue)
        {
            var points = dbcontext.Points.Where(q => q.User.Id == user.Id).ToList();
            int pointsUsed = 0;
            List<E.Point> activatedPoints = new List<E.Point>();
            int totalPointsAvailable = user
                .Points.Where(q => !q.IsUsed && (q.IsExpirable == false || q.ExpireDate >= DateTime.UtcNow))
                .Sum(w => w.Value);
            if (totalPointsAvailable > pointsValue)
            {
                foreach (var item in points)
                {
                    if ((item.IsExpirable == false || item.ExpireDate >= DateTime.UtcNow) && pointsValue > pointsUsed && item.IsUsed == false)
                    {
                        if (item.BalanceLeft > 0)
                        {
                            pointsUsed += Math.Min(item.BalanceLeft, pointsValue - pointsUsed);
                            item.IsUsed = true;
                            activatedPoints.Add(item);
                            item.BalanceLeft -= Math.Min(item.BalanceLeft, pointsValue - pointsUsed);
                        }
                    }
                    if (pointsValue <= pointsUsed)
                        break;
                }
            }
            else
            {
                return null;
            }
            return activatedPoints;
        }

        public (List<(E.CartItem, double)>, E.Point) ApplyPromos(E.Cart cart)
        {
            var promos = dbcontext.Promotions.ToList();
            var point = new E.Point();
            var promoItem = new List<(E.CartItem, double)>();
            List<bool> conditionsResult = new List<bool>();
            foreach (var promotion in promos)
            {
                if (promotion.ActiveTo > DateTime.UtcNow && promotion.ActiveFrom < DateTime.UtcNow)
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
                        conditionsResult.Add(cart.PriceBiggerThan(promotion.MinPay));
                    }
                    if (promotion.IsFirstOrder)
                    {
                        conditionsResult.Add(cart.IsFirstOrder());
                    }
                    if (promotion.Products.Any())
                    {
                        conditionsResult.Add(cart.HasProducts(promotion.Products.Select(q => q.Product)));
                    }
                    if (promotion.BuyQuantity > 0)
                    {
                        conditionsResult.Add(cart.HasProducts(promotion.Products.Select(q => q.Product), promotion.BuyQuantity));
                    }
                    if (!conditionsResult.Contains(false))
                    {
                        if (promotion.DiscountPercent > 0 || promotion.DiscountVal > 0)
                        {
                            promoItem.AddRange(cart.GetDiscountAct(promotion.DiscountVal, promotion.DiscountPercent));
                        }
                        if ((promotion.DiscountPercent > 0 || promotion.DiscountVal > 0) && promotion.PromoProducts.Any())
                        {
                            promoItem.AddRange(
                                cart.GetDiscountForItemtAct(
                                    promotion.PromoProducts.Select(q => q.Product),
                                    promotion.DiscountVal,
                                    promotion.DiscountPercent
                                )
                            );
                        }
                        if (promotion.PointsPercent > 0 || promotion.PointsValue > 0)
                        {
                            point = cart.GetPointsAct(promotion.PointsValue, promotion.PointsPercent, promotion.PointsExpireDays);
                        }
                        if (promotion.PromoProducts.Any() && promotion.GetQuantity > 0)
                        {
                            promoItem.AddRange(cart.GetFreeItemAct(promotion.PromoProducts.Select(q => q.Product), promotion.GetQuantity));
                        }
                    }
                }
            }
            return (promoItem, point);
        }

        public bool IsProcessed(int Id) => dbcontext.Orders.Any(q => q.ID == Id && q.Status == true);
    }
}
