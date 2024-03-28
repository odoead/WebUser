namespace WebUser.features.discount.Exceptions
{
    public class DiscountNotFoundException : Exception
    {
        public DiscountNotFoundException(int id) : base($"Discount with ID {id} doesnt exists")
        {

        }
    }
}
