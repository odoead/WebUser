using WebUser.Domain.exceptions;

namespace WebUser.features.Category.Exceptions
{
    public class CategoryNotFoundException:NotFoundException
    {
        public CategoryNotFoundException(int id):base($"Category with ID {id} doesnt exists")
        {
            
        }
    }
}
