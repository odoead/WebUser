using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Condition
{
    public class ProductsCondition : PromotionCondition<E.Cart>
    {
        public ProductsCondition(IEnumerable<E.Product> products)
            : base(q => q.Items.Select(i => i.Product).Intersect(products).Any())
        /*(q => q.items.Any(i=>products.Contains( i.Product)))*/
        { }
    }

    public static class ProductConditionExtention
    {
        public static bool HasProducts(this E.Cart cart, IEnumerable<E.Product> products)
        {
            var specification = new ProductsCondition(products);
            bool result = specification.ApplyRule(cart);
            return result;
        }

        public static IQueryable<E.Cart> HasProducts(IQueryable<E.Cart> carts, IEnumerable<E.Product> products)
        {
            var specification = new ProductsCondition(products);
            return carts.Where(specification.Expression);
        }
    }
}