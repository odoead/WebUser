namespace WebUser.features.User.DTO;

public class TokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}
