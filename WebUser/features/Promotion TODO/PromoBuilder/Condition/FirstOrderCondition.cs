using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Condition
{
    public class FirstOrderCondition : PromotionCondition<E.Cart>
    {
        public FirstOrderCondition()
            : base(q => q.User.Orders.Count() == 0) { }
    }

    public static class FirstOrderExtention
    {
        public static bool IsFirstOrder(this E.Cart cart)
        {
            var specification = new FirstOrderCondition();
            bool result = specification.ApplyRule(cart);
            return result;
        }

        public static bool IsFirstOrder(this IQueryable<E.Cart> carts)
        {
            var specification = new FirstOrderCondition();
            bool result = specification.ApplyRule(carts.FirstOrDefault());
            return result;
        }

        public static IQueryable<E.Cart> FirstOrder(IQueryable<E.Cart> carts)
        {
            var specification = new FirstOrderCondition();
            return carts.Where(specification.Expression);
        }
    }
}
