using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Actions
{
    public static class GetFreeItem
    {
        public static List<(E.CartItem, double)> GetFreeItemAct(this E.Cart cart, IEnumerable<E.Product> promProducts, int quantity)
        {
            return GenerateFreeItem(cart, promProducts, quantity);
        }

        public static List<(E.CartItem, double)> GetFreeItemAct(this IQueryable<E.Cart> cart, IEnumerable<E.Product> promProducts, int quantity)
        {
            return GenerateFreeItem(cart.FirstOrDefault(), promProducts, quantity);
        }

        private static List<(E.CartItem, double)> GenerateFreeItem(E.Cart cart, IEnumerable<E.Product> promProducts, int quantity)
        {
            var freeItems = new List<(E.CartItem, double)>();
            foreach (var product in promProducts)
            {
                freeItems.Add(
                    (
                        new E.CartItem
                        {
                            Product = product,
                            Cart = cart,
                            Amount = quantity,
                        },
                        product.Price
                    )
                );
            }
            return freeItems;
        }
    }
}
