using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Condition
{
    public class ProductsCondition : PromotionCondition<E.Product>
    {
        public ProductsCondition(IEnumerable<E.Product> Products)
            : base(q => q.Intersect(Products).Any())
        { }
    }

    public static class ProductConditionExtention
    {
        public static bool HasProducts(this ICollection<E.Product> products, IEnumerable<E.Product> conditionProducts)
        {
            var specification = new ProductsCondition(conditionProducts);
            bool result = specification.ApplyRule(products);
            return result;
        }

        public static bool HasProducts(this IQueryable<E.Product> products, IEnumerable<E.Product> conditionProducts)
        {
            var specification = new ProductsCondition(conditionProducts);
            bool result = specification.ApplyRule(products);
            return result;
        }

        public static IQueryable<E.Product> HasProducts(IQueryable<E.Product> products, IQueryable<E.Product> conditionProducts)
        {
            var specification = new ProductsCondition(conditionProducts);
            return products.Where(specification.Expression);
        }
    }
}
