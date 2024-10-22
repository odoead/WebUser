using Swashbuckle.AspNetCore.Filters;

namespace WebUser.features.Promotion.DTO
{
    public class PromotionThumbnailDTOSW : IExamplesProvider<PromotionThumbnailDTO>
    {
        public PromotionThumbnailDTO GetExamples() =>
            new()
            {
                Name = "sale",
                ID = 1,
                ActiveFrom = DateTime.UtcNow.AddDays(10),
                ActiveTo = DateTime.UtcNow.AddDays(50),
                DaysLeft = 50,
                HoursLeft = 0,
                IsActive = false,
            };
    }
}
