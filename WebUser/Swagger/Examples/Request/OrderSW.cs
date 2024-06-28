using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class OrderSW : IExamplesProvider<Order>
    {
        public Order GetExamples() => throw new NotImplementedException();
    }
}
