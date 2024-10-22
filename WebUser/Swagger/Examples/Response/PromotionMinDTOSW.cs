namespace WebUser.features.Promotion_TODO.DTO;

using Swashbuckle.AspNetCore.Filters;

public class PromotionMinDTOSW : IExamplesProvider<PromotionMinDTO>
{
    public PromotionMinDTO GetExamples() => new() { Name = "sale", ID = 5 };
}
