using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Actions
{
    public static class GetPoints
    {
        public static E.Point GetPointsAct(this ICollection<E.Product> products, int pointsValue, int pointsPercent, int expireDays)
        {
            return GenerateDiscounts(products, expireDays, pointsValue, pointsPercent);
        }

        public static E.Point GetPointsAct(this IQueryable<E.Product> products, int pointsValue, int pointsPercent, int expireDays)
        {
            return GenerateDiscounts(products, expireDays, pointsValue, pointsPercent);
        }

        /// <summary>
        /// generate discount point. Generated point is not bound to concrete user
        /// </summary>
        /// <param name="products"></param>
        /// <param name="expireDays"></param>
        /// <param name="pointValue"></param>
        /// <param name="pointPercent"></param>
        /// <returns></returns>
        private static E.Point GenerateDiscounts(this IEnumerable<E.Product> products, int expireDays, int? pointValue = 0, int? pointPercent = 0)
        {
            var value = 0;
            if (pointValue > 0)
            {
                value += pointValue.Value;
            }
            if (pointPercent > 0)
            {
                value += (int)Math.Floor(products.Select(q => q.Price).Sum() * pointPercent.Value / 100);
            }
            var point = new E.Point
            {
                CreateDate = DateTime.UtcNow,
                BalanceLeft = value,
                IsUsed = false,
                Value = value,
            };
            if (expireDays > 0)
            {
                point.IsExpirable = true;
                point.ExpireDate = DateTime.UtcNow.AddDays(expireDays);
            }
            return point;
        }
    }
}
