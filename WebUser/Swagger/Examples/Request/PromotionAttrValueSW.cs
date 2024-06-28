namespace WebUser.Domain.entities;

using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

[Keyless]
public class PromotionAttrValueSW : IExamplesProvider<PromotionAttrValue>
{
    public PromotionAttrValue GetExamples() => throw new NotImplementedException();
}
