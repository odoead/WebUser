namespace WebUser.Domain.entities;

using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

[Keyless]
public class ProductAttributeValueSW : IExamplesProvider<ProductAttributeValue>
{
    public ProductAttributeValue GetExamples() => throw new NotImplementedException();
}
