using System.Linq.Expressions;

namespace WebUser.features.Promotion.PromoBuilder
{
    public abstract class PromotionCondition<T>
    {
        //single obj
        private Func<T, bool> ExpressionFunc => _expressionFunc ?? (_expressionFunc = Expression.Compile());
        public Expression<Func<T, bool>> Expression { get; }
        private Func<T, bool> _expressionFunc;

        public PromotionCondition(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
        }
        public bool ApplyRule(T obj) => ExpressionFunc(obj);

        //array objs
        private Func<IEnumerable<T>, bool> CollectionExpressionFunc =>
            _collectionExpressionFunc ?? (_collectionExpressionFunc = CollectionExpression.Compile());
        public Expression<Func<IEnumerable<T>, bool>> CollectionExpression { get; }
        private Func<IEnumerable<T>, bool> _collectionExpressionFunc;

        public PromotionCondition(Expression<Func<IEnumerable<T>, bool>> collectionExpression)
        {
            CollectionExpression = collectionExpression;
        }

        public bool ApplyRule(IEnumerable<T> objs) => CollectionExpressionFunc(objs);
    }
}
