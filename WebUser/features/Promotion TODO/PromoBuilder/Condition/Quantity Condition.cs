namespace WebUser.features.Promotion.PromoBuilder.Condition
{
    using E = WebUser.Domain.entities;

    public class QuantityCondition : PromotionCondition<E.Cart>
    {
        public QuantityCondition(IEnumerable<E.Product> products, int quantity)
            : base(q => q.Items.Where(q => q.Amount >= quantity).Select(i => i.Product).Intersect(products).Any())
        { }
    }

    public static class QuantityConditionExtention
    {
        public static bool HasProducts(this E.Cart cart, IEnumerable<E.Product> products, int quantity)
        {
            var specification = new QuantityCondition(products, quantity);
            bool result = specification.ApplyRule(cart);
            return result;
        }

        public static bool HasProducts(this IQueryable<E.Cart> carts, IEnumerable<E.Product> products, int quantity)
        {
            var specification = new QuantityCondition(products, quantity);
            bool result = specification.ApplyRule(carts.FirstOrDefault());
            return result;
        }

        public static IQueryable<E.Cart> HasProducts(IQueryable<E.Cart> carts, IQueryable<E.Product> products, int quantity)
        {
            var specification = new QuantityCondition(products, quantity);
            return carts.Where(specification.Expression);
        }
    }
}
