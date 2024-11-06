using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Condition
{
    public class AttributeCondition : PromotionCondition<E.Product>
    {
        public AttributeCondition(IEnumerable<E.AttributeValue> attributes)
            : base(q => q.Any(p => p.AttributeValues.Any(av => attributes.Contains(av.AttributeValue)))) { }
    }

    public static class AttributeConditionExtention
    {
        public static bool HasAttributes(this ICollection<E.Product> products, IEnumerable<E.AttributeValue> attributes)
        {
            var specification = new AttributeCondition(attributes);
            bool result = specification.ApplyRule(products);
            return result;
        }

        public static bool HasAttributes(this IQueryable<E.Product> products, IEnumerable<E.AttributeValue> attributes)
        {
            var specification = new AttributeCondition(attributes);
            bool result = specification.ApplyRule(products);
            return result;
        }

        public static IQueryable<E.Product> HasAttributes(IQueryable<E.Product> products, IQueryable<E.AttributeValue> attributes)
        {
            var specification = new AttributeCondition(attributes);
            return products.Where(specification.Expression);
        }
    }
}
