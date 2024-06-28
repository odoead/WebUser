namespace WebUser.features.User.DTO;

using System.ComponentModel.DataAnnotations;

public class AuthDTO
{
    public string? Username { get; init; }
    public string? Password { get; init; }
}
