using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using WebUser.Domain.entities;
using E = WebUser.Domain.entities;

namespace WebUser.features.Product.extensions
{
    public static class ProductReqestShaper
    {
        public static IQueryable<E.Product> Sort(IQueryable<E.Product> query, int sortType)
        {
            switch (sortType)
            {
                case 0:
                    return query.OrderBy(q => q.Price);
                case 1:
                    return query.OrderByDescending(q => q.Price);
                case 2:
                    return query.OrderBy(q => q.Price);
                default:
                    return query;
            }
        }
        public static IQueryable<E.Product> SearchByAttributeValues(IQueryable<E.Product> query, List<E.AttributeValue> AttributeValues)
        {
            /// Initialize the query with all products
            IQueryable<E.Product> res;

            res = query.Where(product =>
          AttributeValues.All(av =>
              product.AttributeValues.Any(pa =>
                  pa.AttributeName.ID == av.AttributeName.ID &&
                  pa.Value == av.Value)));

            return res;

        }

    }
}
