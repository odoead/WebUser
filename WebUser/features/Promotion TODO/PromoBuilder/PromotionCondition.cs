using System.Linq.Expressions;

namespace WebUser.features.Promotion.PromoBuilder
{
    public abstract class PromotionCondition<T>
    {
        public bool ApplyRule(T obj) => ExpressionFunc(obj);

        public PromotionCondition(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
        }

        public Expression<Func<T, bool>> Expression { get; }
        private Func<T, bool> _expressionFunc;
        private Func<T, bool> ExpressionFunc => _expressionFunc ?? (_expressionFunc = Expression.Compile());
    }
}
