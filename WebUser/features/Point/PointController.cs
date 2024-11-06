namespace WebUser.features.Point;

using System.Net;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.Point.DTO;
using WebUser.features.Point.Functions;
using WebUser.shared;
using WebUser.shared.Action_filter;
using WebUser.shared.RequestForming.features;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class PointController : ControllerBase
{
    private readonly IMediator mediator;

    public PointController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [ValidationFilter]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PointDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create([FromBody] CreatePoint.CreatePointCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not found.");
        }
        command.UserId = userId;

        var result = await mediator.Send(command);
        return CreatedAtRoute("GetPointByID", new { pointId = result.ID }, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        var comm = new DeletePoint.DeletePointCommand { ID = id };
        await mediator.Send(comm);
        return NoContent();
    }

    [HttpGet]
    [Paging]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedList<PointDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll([FromQuery] PointRequestParameters parameters)
    {
        var query = new GetAllPoints.GetAllPointsQuery(parameters);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetPointByID")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PointDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetPointByID(int id)
    {
        var query = new GetPointByID.GetPointByIDQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
