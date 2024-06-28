using WebUser.Domain.exceptions;

namespace WebUser.features.AttributeValue.Exceptions
{
    public class AttributeValueNotFoundException : NotFoundException
    {
        public AttributeValueNotFoundException(int id)
            : base($"AttributeValue with ID {id} doesnt exists") { }
    }
}
