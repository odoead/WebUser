namespace WebUser.features.Promotion.PromoBuilder.Actions
{
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
            var itemDiscounts = new List<(E.CartItem, double)>();
            foreach (var item in cart.Items)
            {
                foreach (var promItem in promProducts)
                {
                    if (item.ProductID == promItem.ID)
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
            }
            return itemDiscounts;
        }
    }
}
