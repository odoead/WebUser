namespace WebUser.features.Promotion.PromoBuilder.Condition
{
    using System.Linq;
    using E = WebUser.Domain.entities;

    public class QuantityCondition : PromotionCondition<(E.Product Product, int Quantity)>
    {
        public QuantityCondition(IEnumerable<E.Product> conditionProducts, int quantity)
            : base(q => q.Any(pq => conditionProducts.Contains(pq.Product) && pq.Quantity >= quantity)) { }
    }

    public static class QuantityConditionExtention
    {
        public static bool HasQuantityProducts(this ICollection<(E.Product Product, int Quantity)> productQuantities, IEnumerable<E.Product> conditionProducts, int quantity
        )
        {
            var specification = new QuantityCondition(conditionProducts, quantity);
            bool result = specification.ApplyRule(productQuantities);
            return result;
        }

        public static bool HasQuantityProducts(this IQueryable<(E.Product Product, int Quantity)> productQuantities, IEnumerable<E.Product> conditionProducts, int quantity
        )
        {
            var specification = new QuantityCondition(conditionProducts, quantity);
            bool result = specification.ApplyRule(productQuantities);
            return result;
        }

        public static IQueryable<(E.Product Product, int Quantity)> HasQuantityProducts(this IQueryable<(E.Product Product, int Quantity)> productQuantities, IQueryable<E.Product> conditionProducts, int quantity
        )
        {
            var specification = new QuantityCondition(conditionProducts, quantity);
            return productQuantities.Where(specification.Expression);
        }
    }
}
