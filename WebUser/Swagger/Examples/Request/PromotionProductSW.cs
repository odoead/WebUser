namespace WebUser.Domain.entities;

using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

[Keyless]
public class PromotionProductSW : IExamplesProvider<PromotionProduct>
{
    public PromotionProduct GetExamples() => throw new NotImplementedException();
}
