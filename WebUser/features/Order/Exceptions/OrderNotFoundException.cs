using WebUser.Domain.exceptions;

namespace WebUser.features.Order.Exceptions
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(int id)
            : base($"Order with ID {id} doesnt exists") { }
    }
}
