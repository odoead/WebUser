namespace WebUser.features.Point;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.OrderProduct.DTO;
using WebUser.features.Point.DTO;
using WebUser.features.Point.Functions;
using WebUser.shared;

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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(PointDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create([FromBody] CreatePoint.CreatePointCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetPointByID", new { pointId = result.ID }, result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        var comm = new DeletePoint.DeletePointCommand { ID = id, };
        await mediator.Send(comm);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<PointDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var query = new GetAllPoints.GetAllPointsQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetPointByID")]
    [ProducesResponseType(typeof(PointDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetPointByID(int id)
    {
        var query = new GetPointByID.GetPointByIDQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
