namespace WebUser.features.Coupon.DTO;

using Swashbuckle.AspNetCore.Filters;

public class CouponMinDTOSW : IExamplesProvider<CouponMinDTO>
{
    public CouponMinDTO GetExamples() =>
        new()
        {
            ID = 3,
            IsActive = true,
            DiscountPercent = 50,
            DiscountVal = 0,
        };
}
