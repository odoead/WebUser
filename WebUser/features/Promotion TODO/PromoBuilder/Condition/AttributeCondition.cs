using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Condition
{
    public class AttributeCondition : PromotionCondition<E.Cart>
    {
        public AttributeCondition(IEnumerable<E.AttributeValue> attributes)
            : base(q => q.Items.SelectMany(i => i.Product.AttributeValues.Select(q => q.AttributeValue)).Intersect(attributes).Any())
        //(q => q.items.Any(item => item.Product.AttributeValues.Any(attr => Attributes.Contains(attr))))
        { }
    }

    public static class AttributeConditionExtention
    {
        public static bool HasAttributes(this E.Cart cart, IEnumerable<E.AttributeValue> attributes)
        {
            var specification = new AttributeCondition(attributes);
            bool result = specification.ApplyRule(cart);
            return result;
        }

        public static IQueryable<E.Cart> HasAttributes(IQueryable<E.Cart> carts, IEnumerable<E.AttributeValue> attributes)
        {
            var specification = new AttributeCondition(attributes);
            return carts.Where(specification.Expression);
        }
    }
}
