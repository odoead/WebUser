namespace WebUser.features.User;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.Domain.entities;
using WebUser.features.User.DTO;
using WebUser.features.User.Functions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;

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
    /// еуеееееееееее
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
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
            return BadRequest();
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Authenticate([FromBody] AuthDTO command)
    {
        if (!await service.User.ValidateUser(command.Password, command.Username))
        {
            return Unauthorized();
        }
        var tokenDTO = await service.User.CreateToken(true);
        return Ok(tokenDTO);
    }
}
