namespace WebUser.features.Promotion_TODO.DTO;

using Swashbuckle.AspNetCore.Filters;

public class PromotionProductPageDTOSW : IExamplesProvider<PromotionProductPageDTO>
{
    public PromotionProductPageDTO GetExamples() => new PromotionProductPageDTO { Name = "sale", Description = "buy one get two promo" };
}
