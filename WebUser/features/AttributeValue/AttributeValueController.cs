namespace WebUser.features.AttributeValue;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.AttributeValue.functions;
using WebUser.shared;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class AttributeValueController : ControllerBase
{
    private readonly IMediator mediator;

    public AttributeValueController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(AttributeValueDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create([FromBody] CreateAttrValue.CreateAttributeValueCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetAttributeValueByID", new { attributeValueId = result.ID }, result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        var comm = new DeleteAttrValue.DeleteAttributeValueCommand { ID = id };
        await mediator.Send(comm);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<AttributeValueDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var comm = new GetAllAttrValues.GetAllAttrValueQuery();
        var result = await mediator.Send(comm);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetAttributeValueByID")]
    [ProducesResponseType(typeof(AttributeValueDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByID(int id)
    {
        var comm = new GetByIDAttributeValue.GetByIDAttrValueQuery { Id = id };
        var result = await mediator.Send(comm);
        return Ok(result);
    }
}
