namespace WebUser.features.User;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebUser.features.User.DTO;
using WebUser.features.User.Functions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using static WebUser.features.User.Functions.SetNotifier;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class UserController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IServiceWrapper service;

    public UserController(IMediator mediator, IServiceWrapper serviceWrapper)
    {
        this.mediator = mediator;
        service = serviceWrapper;
    }

    /// <summary>
    /// register new account
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidationFilter]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Register([FromBody] CreateUser.CreateUserCommand command)
    {
        var result = await mediator.Send(command);
        if (result.Succeeded)
        {
            return Ok();
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenDTO), 200)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult> Authenticate([FromBody] AuthDTO command)
    {
        if (!await service.User.ValidateUser(command.Password, command.Email))
        {
            return Unauthorized();
        }
        var name = await service.User.GetNameByEmail(command.Email);
        var tokenDTO = await service.User.CreateToken(true);
        return Ok(tokenDTO);
    }

    [HttpPost("setNotifier")]
    [Authorize(Roles = "User")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> SetNotifier([FromBody] SetNotifierCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}
