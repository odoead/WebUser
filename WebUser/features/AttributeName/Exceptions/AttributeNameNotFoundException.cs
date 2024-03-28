using WebUser.Domain.exceptions;

namespace WebUser.features.AttributeName.Exceptions
{
    public class AttributeNameNotFoundException: NotFoundException
    {
        public AttributeNameNotFoundException(int id): base($"AttributeName with ID {id} doesnt exists")
        {
            
        }
    }
}
