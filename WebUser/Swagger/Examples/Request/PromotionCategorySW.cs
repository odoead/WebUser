namespace WebUser.Domain.entities;

using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

[Keyless]
public class PromotionCategorySW : IExamplesProvider<PromotionCategory>
{
    public PromotionCategory GetExamples() => throw new NotImplementedException();
}
