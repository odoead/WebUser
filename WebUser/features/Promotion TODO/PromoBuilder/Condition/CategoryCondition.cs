using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Condition
{
    public class CategoryCondition : PromotionCondition<E.Cart>
    {
        public CategoryCondition(IEnumerable<E.Category> categories)
            : base(q =>
                q.Items.SelectMany(i => i.Product.AttributeValues.Select(q => q.AttributeValue))
                    .Intersect(categories.SelectMany(w => w.Attributes.SelectMany(e => e.AttributeName.AttributeValues)))
                    .Any()
            )
        //(q => q.items.Any(item => item.Product.AttributeValues.Any(attr => categories.Contains(attr.AttributeName.Category))))
        { }
    }

    public static class CategoryConditionExtention
    {
        public static bool HasCategories(this E.Cart cart, IEnumerable<E.Category> categories)
        {
            var specification = new CategoryCondition(categories);
            bool result = specification.ApplyRule(cart);
            return result;
        }

        public static IQueryable<E.Cart> HasCategories(IQueryable<E.Cart> carts, IEnumerable<E.Category> categories)
        {
            var specification = new CategoryCondition(categories);
            return carts.Where(specification.Expression);
        }
    }
}
