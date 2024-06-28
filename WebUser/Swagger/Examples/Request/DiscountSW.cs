using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class DiscountSW : IExamplesProvider<Discount>
    {
        public Discount GetExamples() => throw new NotImplementedException();
    }
}
