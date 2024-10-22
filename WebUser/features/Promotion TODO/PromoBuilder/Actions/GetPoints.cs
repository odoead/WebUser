using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Actions
{
    public static class GetPoints
    {
        public static E.Point GetPointsAct(this E.Cart cart, int pointsValue, int pointsPercent, int expireDays)
        {
            return GeneratePoint(cart, pointsValue, pointsPercent, expireDays);
        }

        public static E.Point GetPointsAct(this IQueryable<E.Cart> cart, int pointsValue, int pointsPercent, int expireDays)
        {
            return GeneratePoint(cart.FirstOrDefault(), pointsValue, pointsPercent, expireDays);
        }

        private static E.Point GeneratePoint(E.Cart cart, int pointValue, int pointPercent, int expireDays)
        {
            var user = cart.User;
            int value = 0;
            if (pointValue > 0)
            {
                value = pointValue;
            }
            if (pointPercent > 0)
            {
                value = (int)cart.Items.Select(q => q.Product.Price).Sum() * pointPercent / 100;
            }
            var point = new E.Point
            {
                CreateDate = DateTime.UtcNow,
                BalanceLeft = value,
                IsUsed = false,
                UserID = cart.UserID,
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
