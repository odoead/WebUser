namespace WebUser.features.Promotion.PromoBuilder.Actions
{
    using System.Collections.Generic;
    using E = WebUser.Domain.entities;

    public static class GetDiscountForItems
    {
        public static List<(E.CartItem, double)> GetDiscountForItemtAct(
            this E.Cart cart,
            IEnumerable<E.Product> promProducts,
            double discountValue,
            float discountPercent
        )
        {
            return GenerateDiscount(cart, promProducts, discountValue, discountPercent);
        }

        public static List<(E.CartItem, double)> GetDiscountForItemtAct(
            this IQueryable<E.Cart> cart,
            IEnumerable<E.Product> promProducts,
            double discountValue,
            float discountPercent
        )
        {
            return GenerateDiscount(cart.FirstOrDefault(), promProducts, discountValue, discountPercent);
        }

        private static List<(E.CartItem, double)> GenerateDiscount(
            E.Cart cart,
            IEnumerable<E.Product> promProducts,
            double discountValue,
            float discountPercent
        )
        {
            var itemDiscounts = new List<(E.CartItem, double)>();
            var promoProductIds = new HashSet<int>(promProducts.Select(p => p.ID));
            foreach (var item in cart.Items)
            {
                if (promoProductIds.Contains(item.ProductID))
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
            }

            return itemDiscounts;
        }
    }
}
