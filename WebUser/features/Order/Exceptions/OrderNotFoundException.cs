namespace WebUser.features.Order.Exceptions
{
    public class OrderNotFoundException:Exception
    {
        public OrderNotFoundException(int id) : base($"Order with ID {id} doesnt exists")
        {

        }
    }
}
