using Swashbuckle.AspNetCore.Filters;
using WebUser.features.Product.DTO;

namespace WebUser.features.discount.DTO
{
    public class DiscountDTOSW : IExamplesProvider<DiscountDTO>
    {
        public DiscountDTO GetExamples() =>
            new()
            {
                DiscountVal = 50,
                DiscountPercent = 0,
                IsActive = true,
                ID = 3,
                ActiveFrom = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddDays(50),
                CreatedAt = DateTime.UtcNow,
                Product = new ProductMinDTO
                {
                    ID = 1,
                    Name = "table",
                    Price = 500,
                },
            };
    }
}
