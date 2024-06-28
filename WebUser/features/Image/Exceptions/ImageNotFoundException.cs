using WebUser.Domain.exceptions;

namespace WebUser.features.Image.Exceptions
{
    public class ImageNotFoundException : NotFoundException
    {
        public ImageNotFoundException(int id)
            : base($"Image with ID {id} doesnt exists") { }
    }
}
