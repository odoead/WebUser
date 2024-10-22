namespace WebUser.Swagger.Examples.Response;

using Swashbuckle.AspNetCore.Filters;
using WebUser.features.User.DTO;

public class TokenDTOSW : IExamplesProvider<TokenDTO>
{
    public TokenDTO GetExamples() =>
        new TokenDTO
        {
            AccessToken = "12345",
            RefreshToken = "a1b2c3",
            Email = "commonUser@email.com",
            Name = "Kirill Muraviov ",
        };
}
