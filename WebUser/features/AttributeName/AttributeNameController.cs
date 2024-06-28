namespace WebUser.features.AttributeName;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeName.functions;
using WebUser.shared;

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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(AttributeNameDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create([FromBody] CreateAttributeName.CreateAttributeNameCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetAttributeNameByID", new { attributeNameId = result.Id }, result);
    }

    [HttpPatch("{id:int}/value/add")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> AddValue(int id, [FromBody] string value)
    {
        var command = new AddAttributeValueToAttrName.AddAttributeValueCommand { AttributeValue = value, AttributeNameID = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("{id:int}/value/{valueid:int}/remove")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> RemoveValue(int id, int valueid)
    {
        var command = new RemoveAttributeValueFromAttrName.DeleteAttributeValueCommand { AttributeNameId = id, AttributeValueId = valueid };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        var comm = new DeleteAttributeName.DeleteAttributeNameCommand { ID = id };
        await mediator.Send(comm);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<AttributeNameDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var comm = new GetAllAttrNameAsync.GetAllAttrNameQuery();
        var result = await mediator.Send(comm);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetAttributeNameByID")]
    [ProducesResponseType(typeof(AttributeNameDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByID(int id)
    {
        var comm = new GetByIDAttributeName.GetByIDAttrNameQuery { Id = id };
        var result = await mediator.Send(comm);
        return Ok(result);
    }
}
