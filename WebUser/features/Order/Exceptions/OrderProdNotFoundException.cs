using WebUser.Domain.exceptions;

namespace WebUser.features.Order.Exceptions
{
    public class OrderProductNotFoundException : NotFoundException
    {
        public OrderProductNotFoundException(int id)
            : base($"Order product with ID {id} doesnt exists") { }
    }
}
