using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Condition
{
    public class SpendCondition : PromotionCondition<E.Cart>
    {
        public SpendCondition(double minCost)
            : base(q => q.Items.Select(q => q.Product.Price).Sum() > minCost) { }
    }

    public static class CoustConditionExtention
    {
        public static bool PriceBiggerThan(this E.Cart cart, double minCost)
        {
            var specification = new SpendCondition(minCost);
            bool result = specification.ApplyRule(cart);
            return result;
        }

        public static IQueryable<E.Cart> PriceBiggerThan(IQueryable<E.Cart> carts, int minCost)
        {
            var specification = new SpendCondition(minCost);
            return carts.Where(specification.Expression);
        }
    }
}
