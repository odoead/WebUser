namespace WebUser.Domain.entities;

using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

[Keyless]
public class PromotionPromProductSW : IExamplesProvider<PromotionPromProduct>
{
    public PromotionPromProduct GetExamples() => throw new NotImplementedException();
}
