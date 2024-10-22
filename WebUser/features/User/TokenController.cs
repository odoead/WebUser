namespace WebUser.features.User;

using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.User.DTO;
using WebUser.shared.RepoWrapper;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class TokenController : ControllerBase
{
    private readonly IServiceWrapper service;

    public TokenController(IServiceWrapper service)
    {
        this.service = service;
    }

    [HttpPost("refresh")]
    [Authorize]
    [ProducesResponseType(typeof(TokenDTO), 200)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult> RefreshToken([FromBody] TokenDTO tokenDto)
    {
        try
        {
            var newTokenDto = await service.User.RefreshToken(tokenDto);
            return Ok(newTokenDto);
        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }
}
