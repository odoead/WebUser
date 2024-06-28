using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class ProductSW : IExamplesProvider<Product>
    {
        public Product GetExamples() => throw new NotImplementedException();
    }
}
