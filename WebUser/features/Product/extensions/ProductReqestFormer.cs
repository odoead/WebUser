using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebUser.features.Product.Enums;
using E = WebUser.Domain.entities;

namespace WebUser.features.Product.extensions
{
    public static class ProductReqestFormer
    {
        /// <summary>
        /// sorts products by an orderBy. OrderBy accept ProductOrderBy enum params
        /// </summary>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IQueryable<E.Product> Sort(this IQueryable<E.Product> query, string orderBy)
        {
            ProductOrderBy tryParseResult;
            if (Enum.TryParse(orderBy, out tryParseResult))
            {
                switch (tryParseResult)
                {
                    case ProductOrderBy.NameAsc:
                        return query.OrderBy(x => x.Name);
                    case ProductOrderBy.NameDesc:
                        return query.OrderByDescending(x => x.Name);
                    case ProductOrderBy.PriceDesc:
                        return query.OrderByDescending(x => x.Price);
                    case ProductOrderBy.PriceAsc:
                        return query.OrderBy(x => x.Price);
                    case ProductOrderBy.DateCreated:
                        return query.OrderByDescending(x => x.DateCreated);
                    default:
                        break;
                }
            }
            return query;
        }

        /// <summary>
        /// searches for products, which name contains every requested word in the requestName string(words splits automatically)
        /// </summary>
        /// <param name="products"></param>
        /// <param name="requestName"></param>
        /// <returns></returns>
        public static IQueryable<E.Product> SearchByName(this IQueryable<E.Product> products, string? requestName)
        {

            if (string.IsNullOrEmpty(requestName))
                return products;

            char[] splitChars = { ' ', ',', ';', '-', '_', '.', ':', '\t' };
            var searchTerms = requestName.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

            var query = products;
            // Build the expression outside the query
            Expression<Func<E.Product, bool>> searchExpression = p => true;

            foreach (var term in searchTerms)
            {
                string searchTerm = term; // Create a local variable to capture in the expression
                query = query.Where(p => EF.Functions.Like(p.Name ?? "", $"%{searchTerm}%"));
            }

            return query;
        }

        /// <summary>
        /// filter products by attributeValue Id's
        /// </summary>
        /// <param name="products"></param>
        /// <param name="attributeValueIDs"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public static IQueryable<E.Product> Filter(this IQueryable<E.Product> products, List<int> attributeValueIDs, int minPrice = 0, int maxPrice = int.MaxValue)
        {
            var query = products.Where(q => q.Price >= minPrice && q.Price <= maxPrice);

            if (!attributeValueIDs.Any())
                return query;

            var requiredCount = attributeValueIDs.Count;

            return query.Where(product => product.AttributeValues.Count(av => attributeValueIDs.Contains(av.AttributeValueID)) == requiredCount);
        }
    }
}
