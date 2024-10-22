using WebUser.PricingService.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Actions
{
    public static class GetDiscountCart
    {
        public static List<(E.CartItem, double)> GetDiscountAct(this E.Cart cart, double discountValue, float discountPercent) =>
            GenerateDiscounts(cart, discountValue, discountPercent);

        public static List<(E.CartItem, double)> GetDiscountAct(this IQueryable<E.Cart> cartQuery, double discountValue, float discountPercent) =>
            GenerateDiscounts(cartQuery.FirstOrDefault(), discountValue, discountPercent);

        private static List<(E.CartItem, double)> GenerateDiscounts(E.Cart cart, double discountValue, float discountPercent)
        {
            var itemDiscounts = new List<(E.CartItem, double)>();
            var cartItems = cart.Items;
            foreach (var item in cartItems)
            {
                var basePrice = item.Product.Price;
                double discountVal = 0;
                if (discountValue > 0)
                {
                    discountVal += discountValue;
                }
                if (discountPercent > 0)
                {
                    discountVal += basePrice * discountPercent / 100;
                }
                itemDiscounts.Add((item, discountVal));
            }
            return itemDiscounts;
        }
        
    }
}
