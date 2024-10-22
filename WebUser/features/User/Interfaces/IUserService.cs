namespace WebUser.features.User.Interfaces;

using WebUser.features.User.DTO;

public interface IUserService
{
    //public Task<string> CreateToken();
    public Task<bool> ValidateUser(string password, string email);
    public Task<TokenDTO> CreateToken(bool populationExp);
    public Task<TokenDTO> RefreshToken(TokenDTO tokenDTO);
    public Task<string> GetNameByEmail(string email);
}
