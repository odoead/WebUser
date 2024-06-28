using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class UserSW : IExamplesProvider<User>
    {
        public User GetExamples() => throw new NotImplementedException();
    }
}
