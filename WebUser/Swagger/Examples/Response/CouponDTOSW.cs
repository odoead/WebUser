using Swashbuckle.AspNetCore.Filters;
using WebUser.features.Product.DTO;

namespace WebUser.features.Coupon.DTO
{
    public class CouponDTOSW : IExamplesProvider<CouponDTO>
    {
        public CouponDTO GetExamples() =>
            new()
            {
                ActiveFrom = DateTime.UtcNow.AddDays(10),
                ID = 1,
                ActiveTo = DateTime.UtcNow.AddDays(100),
                Code = "r5tgjl",
                CreatedAt = DateTime.UtcNow,
                DiscountPercent = 32,
                DiscountVal = 56.3,
                IsActivated = false,
                IsActive = true,
                Product = new ProductMinDTO
                {
                    ID = 5,
                    Name = "smartphone",
                    Price = 1500,
                },
            };
    }
}
