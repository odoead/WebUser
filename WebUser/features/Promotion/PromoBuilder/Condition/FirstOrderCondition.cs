using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Condition
{
    public class FirstOrderCondition : PromotionCondition<E.User>
    {
        public FirstOrderCondition()
            : base(q => q.Orders.Count() == 0) { }
    }

    public static class FirstOrderExtention
    {
        public static bool IsFirstOrder(this E.User users)
        {
            var specification = new FirstOrderCondition();
            bool result = specification.ApplyRule(users);
            return result;
        }

        public static bool IsFirstOrder(this IQueryable<E.User> users)
        {
            var specification = new FirstOrderCondition();
            bool result = specification.ApplyRule(users);
            return result;
        }

        public static IQueryable<E.User> FirstOrder(IQueryable<E.User> users)
        {
            var specification = new FirstOrderCondition();
            return users.Where(specification.Expression);
        }
    }
}
