using System.Linq.Expressions;
using E = WebUser.Domain.entities;

namespace WebUser.shared.RequestForming.features
{
    public static class RequestShapingWrong
    {
        public static IQueryable<T> Filter<T>(IQueryable<T> source, Expression<Func<T, bool>> filterExpression)
        {
            return source.Where(filterExpression);
        }

        /*public static IQueryable<T> Sort<T>(IQueryable<T> source, Expression<Func<T, bool>> sortExpression, bool ascending=true)
        {
            if (ascending)
            {
                return source.OrderBy(sortExpression);
            }
            else
            {
                return source.OrderByDescending(sortExpression);
            }
        }*/

        public static IQueryable<T> Search<T>(IQueryable<T> query, string searchString, params Expression<Func<T, string>>[] searchProperties)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return query;
            }
            // Combine multiple search properties into one expression
            Expression<Func<T, bool>> combinedSearchExpression = null;
            foreach (var propertyExpression in searchProperties)
            {
                var propertySearchExpression = BuildSearchExpression(propertyExpression, searchString);
                if (combinedSearchExpression == null)
                {
                    combinedSearchExpression = propertySearchExpression;
                }
                else
                {
                    combinedSearchExpression = combinedSearchExpression.Or(propertySearchExpression);
                }

            }

            return query.Where(combinedSearchExpression);
        }

        private static Expression<Func<T, bool>> BuildSearchExpression<T>(Expression<Func<T, string>> propertyExpression, string searchString)
        {
            var parameter = propertyExpression.Parameters.First();
            var body = propertyExpression.Body;
            var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            body = Expression.Call(body, toLowerMethod);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var constant = Expression.Constant(searchString.ToLower(), typeof(string));
            var call = Expression.Call(body, containsMethod, constant);
            return Expression.Lambda<Func<T, bool>>(call, parameter);

        }

    }
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
