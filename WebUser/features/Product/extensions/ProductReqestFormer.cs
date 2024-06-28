using Microsoft.EntityFrameworkCore;
using WebUser.features.Product.Enums;
using E = WebUser.Domain.entities;

namespace WebUser.features.Product.extensions
{
    public static class ProductReqestFormer
    {
        //pattern:
        //category0/category1/category1.2/attributeName1=attributeValueName1,attributeValueName2&attributeName3=attributeValueName6,attributeValueName7&range=100-200
        //page
        //ordrby
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

        public static IQueryable<E.Product> SearchByName(this IQueryable<E.Product> products, string? requestName)
        {
            char[] splitChars = { ' ', ',', ';', '-', '_', '.', ':', '\t' };
            List<string> input = requestName.ToLower().Split(splitChars).ToList();
            return string.IsNullOrEmpty(requestName)
                ? products
                : products.Where(q => q.Name != null && input.All(item => q.Name.ToLower().Contains(item)));
        }

        public static IQueryable<E.Product> Filter(
            this IQueryable<E.Product> products,
            List<int> attributeValueIDs,
            int minPrice = 0,
            int maxPrice = int.MaxValue
        ) =>
            products.Where(q =>
                attributeValueIDs.All(id => q.AttributeValues.Select(p => p.AttributeValueID).Contains(id))
                && q.Price >= minPrice
                && q.Price <= maxPrice
            );
        /*public static IQueryable<E.Product> FilterByAttributeValues(IQueryable<E.Product> query, string request)
        {
            var categories = request.Split('/').ToList();
            List<(string attrName, List<string> attrValues)> attrFilter = new List<(string, List<string>)>();
            var priceRange = new Tuple<int, int>(0, 0);
            foreach (var part in request.Split("/"))
            {
                if (part.StartsWith("range="))
                {
                    var rangeParts = part.Substring(6).Split('-');
                    priceRange = Tuple.Create(int.Parse(rangeParts[0]), int.Parse(rangeParts[1]));
                }
                //else if()
                else
                {
                    var keyValue = part.Split('=');
                    var attrName = keyValue[0];
                    List<string> attrValue = keyValue[1].Split(',').ToList();
                    attrFilter.Add((attrName, attrValue));
                }
            }
            query
                .Include(p => p.AttributeValues)
                .ThenInclude(q => q.AttributeValue)
                .ThenInclude(q => q.AttributeName)
                .ThenInclude(w => w.Category)
                .Where(p =>
                    categories.Contains(p.AttributeValues.SelectMany(a => a.AttributeValue.AttributeName.Category.Name))
                    && p.Price >= priceRange.Item1
                    && p.Price <= priceRange.Item2
                );
            foreach (var filter in attrFilter)
            {
                query = query.Where(q =>
                    q.AttributeValues.Any(a =>
                        a.AttributeValue.AttributeName.Name == filter.attrName && filter.attrValues.Contains(a.AttributeValue.Value)
                    )
                );
            }
            return query;
        }*/
    }
}
