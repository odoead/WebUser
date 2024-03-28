namespace WebUser.features.Cart.Exceptions
{
    public class CartNotFoundException:Exception
    {
        public CartNotFoundException(int id) : base($"Cart with ID {id} doesnt exists")
        {

        }
    }
}
