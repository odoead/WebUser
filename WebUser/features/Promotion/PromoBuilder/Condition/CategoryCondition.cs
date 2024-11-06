using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Condition
{
    public class CategoryCondition : PromotionCondition<E.Product>
    {
        public CategoryCondition(IEnumerable<E.Category> categories)
            : base(q => q.Any(p => p.AttributeValues.Any(av => av.AttributeValue.AttributeName.Categories.Any(c => categories.Contains(c.Category)))))
        { }
    }

    public static class CategoryConditionExtention
    {
        public static bool HasCategories(this ICollection<E.Product> products, IEnumerable<E.Category> categories)
        {
            var specification = new CategoryCondition(categories);
            bool result = specification.ApplyRule(products);
            return result;
        }

        public static bool HasCategories(this IQueryable<E.Product> products, IEnumerable<E.Category> categories)
        {
            var specification = new CategoryCondition(categories);
            bool result = specification.ApplyRule(products);
            return result;
        }

        public static IQueryable<E.Product> HasCategories(IQueryable<E.Product> products, IQueryable<E.Category> categories)
        {

            var specification = new CategoryCondition(categories);
            return products.Where(specification.Expression);
        }
    }
}
