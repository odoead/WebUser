namespace WebUser.features.User.Interfaces;

using WebUser.features.User.DTO;

public interface IUserService
{
    //public Task<string> CreateToken();
    public Task<bool> ValidateUser(string password, string username);
    public Task<TokenDTO> CreateToken(bool populationExp);
    public Task<TokenDTO> RefreshToken(TokenDTO tokenDTO);
}
