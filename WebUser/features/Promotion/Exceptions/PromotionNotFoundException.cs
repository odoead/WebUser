using WebUser.Domain.exceptions;

namespace WebUser.features.Promotion.Exceptions
{
    public class PromotionNotFoundException : NotFoundException
    {
        public PromotionNotFoundException(int id)
            : base($"Promotion with ID {id} doesnt exists") { }
    }
}
