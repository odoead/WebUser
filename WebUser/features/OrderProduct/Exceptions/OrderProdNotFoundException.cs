namespace WebUser.features.OrderProduct.Exceptions
{
    public class OrderProductNotFoundException:Exception
    {
        public OrderProductNotFoundException(int id) : base($"Order product with ID {id} doesnt exists")
        {

        }
    }
}
