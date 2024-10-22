namespace WebUser.features.Discount.DTO;

using Swashbuckle.AspNetCore.Filters;

public class DiscountMinDTOSW : IExamplesProvider<DiscountMinDTO>
{
    public DiscountMinDTO GetExamples() =>
        new()
        {
            ID = 10,
            DiscountPercent = 23,
            DiscountVal = 0,
            IsActive = true,
        };
}
