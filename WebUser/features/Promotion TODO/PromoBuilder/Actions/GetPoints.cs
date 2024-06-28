using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Actions
{
    public static class GetPoints
    {
        public static E.Point GetPointsAct(this E.Cart cart, int pointsValue, int pointsPercent, int expireDays)
        {
            var user = cart.User;
            int value = 0;
            if (pointsValue > 0)
            {
                value = pointsValue;
            }
            if (pointsPercent > 0)
            {
                value = (int)cart.Items.Select(q => q.Product.Price).Sum() * pointsPercent / 100;
            }
            var point = new E.Point
            {
                CreateDate = DateTime.UtcNow,
                BalanceLeft = value,
                IsUsed = false,
                UserID = cart.UserID,
                Value = (int)value,
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
