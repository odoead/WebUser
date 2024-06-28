using WebUser.Domain.exceptions;

namespace WebUser.features.discount.Exceptions
{
    public class DiscountNotFoundException : NotFoundException
    {
        public DiscountNotFoundException(int id)
            : base($"Discount with ID {id} doesnt exists") { }
    }
}
