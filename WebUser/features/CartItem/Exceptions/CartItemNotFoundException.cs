using WebUser.Domain.exceptions;

namespace WebUser.features.CartItem.Exceptions
{
    public class CartItemNotFoundException : NotFoundException
    {
        public CartItemNotFoundException(int cartItemId)
            : base($"Product doesnt exists in cart {cartItemId}") { }
    }
}
