namespace WebUser.features.User;

using Microsoft.AspNetCore.Mvc;
using WebUser.features.User.DTO;
using WebUser.shared.RepoWrapper;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly IServiceWrapper service;

    public TokenController(IServiceWrapper service)
    {
        this.service = service;
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDTO tokenDTO)
    {
        var tokenRes = await service.User.RefreshToken(tokenDTO);
        return Ok(tokenRes);
    }
}
