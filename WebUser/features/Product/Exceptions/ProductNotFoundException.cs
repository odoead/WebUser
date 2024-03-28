namespace WebUser.features.Product.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(int id) : base($"Product with ID {id} doesnt exists\")")
        {

        }
    }
}
