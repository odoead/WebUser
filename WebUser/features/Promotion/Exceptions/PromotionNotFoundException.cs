namespace WebUser.features.Promotion.Exceptions
{
    public class PromotionNotFoundException:Exception
    {
        public PromotionNotFoundException(int id) : base($"Promotion with ID {id} doesnt exists")
        {

        }
    }
}
