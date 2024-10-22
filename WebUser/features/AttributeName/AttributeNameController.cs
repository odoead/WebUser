namespace WebUser.features.AttributeName;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeName.functions;
using WebUser.shared;
using WebUser.shared.Action_filter;
using WebUser.shared.RequestForming.features;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class AttributeNameController : ControllerBase
{
    private readonly IMediator mediator;

    public AttributeNameController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    // [Authorize(Roles = "Admin")]
    [ValidationFilter]
    [ProducesResponseType(typeof(AttributeNameDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create([FromBody] CreateAttributeName.CreateAttributeNameCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetAttributeNameByID", new { id = result.Id }, result);
    }

    [HttpPost("{id:int}/value")]
    //[Authorize(Roles = "Admin")]
    [ValidationFilter]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> AddValue(int id, [FromBody] string value)
    {
        var command = new AddAttributeValueToAttrName.AddAttributeValueCommand { AttributeValue = value, AttributeNameID = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}/value/{valueid:int}")]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> RemoveValue(int id, int valueid)
    {
        var command = new RemoveAttributeValueFromAttrName.DeleteAttributeValueCommand { AttributeNameId = id, AttributeValueId = valueid };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        var comm = new DeleteAttributeName.DeleteAttributeNameCommand { ID = id };
        await mediator.Send(comm);
        return NoContent();
    }

    [HttpGet]
    [Paging]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedList<AttributeNameDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll([FromQuery] AttributeNameRequestParameters parameters)
    {
        var query = new GetAllAttrNameAsync.GetAllAttrNameQuery { Parameters = parameters };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetAttributeNameByID")]
    // [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(AttributeNameDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByID(int id)
    {
        var comm = new GetByIDAttributeName.GetByIDAttrNameQuery { Id = id };
        var result = await mediator.Send(comm);
        return Ok(result);
    }
}
