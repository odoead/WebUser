using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class CartItemSW : IExamplesProvider<CartItem>
    {
        public CartItem GetExamples() => throw new NotImplementedException();
    }
}
