using Swashbuckle.AspNetCore.Filters;

namespace WebUser.features.Point.DTO
{
    public class PointDTOSW : IExamplesProvider<PointDTO>
    {
        public PointDTO GetExamples() =>
            new()
            {
                ID = 1,
                Value = 100,
                BalanceLeft = 80,
                isExpirable = true,
                IsUsed = false,
                IsActive = true,
                CreateDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.AddDays(30),
                UserID = "user123",
                OrderID = null,
            };
    }
}
