namespace WebUser.Swagger.Examples.Request;

using Swashbuckle.AspNetCore.Filters;
using WebUser.features.User.DTO;

public class AuthDTOSW : IExamplesProvider<AuthDTO>
{
    public AuthDTO GetExamples() => new() { Password = "12345", Email = "CommonUser@email.com" };
}
