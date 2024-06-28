using WebUser.Domain.exceptions;

namespace WebUser.features.Point.Exceptions
{
    public class PointNotFoundException : NotFoundException
    {
        public PointNotFoundException(int id)
            : base($"Point with ID {id} doesnt exists") { }
    }
}
