using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class CartSW : IExamplesProvider<Cart>
    {
        public Cart GetExamples() => throw new NotImplementedException();
    }
}
