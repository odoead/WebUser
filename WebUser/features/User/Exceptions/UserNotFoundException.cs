using WebUser.Domain.exceptions;

namespace WebUser.features.User.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string id)
            : base("User with id " + id + " not found") { }
    }
}
