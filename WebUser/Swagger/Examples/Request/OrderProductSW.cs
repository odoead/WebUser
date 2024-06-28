using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class OrderProductSW : IExamplesProvider<OrderProduct>
    {
        public OrderProduct GetExamples() => throw new NotImplementedException();
    }
}
