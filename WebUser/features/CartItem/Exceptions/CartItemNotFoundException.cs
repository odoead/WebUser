namespace WebUser.features.CartItem.Exceptions
{
    public class CartItemNotFoundException:Exception
    {
        public CartItemNotFoundException(int id) : base($"Cart item with ID {id} doesnt exists")
        {

        }
    }
}
