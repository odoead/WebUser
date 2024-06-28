using WebUser.Domain.exceptions;

namespace WebUser.features.Cart.Exceptions
{
    public class CartNotFoundException : NotFoundException
    {
        public CartNotFoundException(int id)
            : base($"Cart with ID {id} doesnt exists") { }
    }
}
