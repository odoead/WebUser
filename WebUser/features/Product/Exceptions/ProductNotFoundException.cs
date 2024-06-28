using WebUser.Domain.exceptions;

namespace WebUser.features.Product.Exceptions
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(int id)
            : base($"Product with ID {id} doesnt exists\")") { }
    }
}
