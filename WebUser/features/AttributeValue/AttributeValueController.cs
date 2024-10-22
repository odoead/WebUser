namespace WebUser.features.AttributeValue;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.AttributeValue.functions;
using WebUser.shared;
using WebUser.shared.Action_filter;
using WebUser.shared.RequestForming.features;
using static WebUser.features.AttributeValue.functions.UpdateAttributeValue;

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

    [HttpGet]
    [Paging]
    [Authorize(Roles = "Admin")]

    [ProducesResponseType(typeof(PagedList<AttributeValueDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll([FromQuery] AttributeValuesRequestParameters parameters)
    {
        var comm = new GetAllAttrValues.GetAllAttrValueQuery(parameters);
        var result = await mediator.Send(comm);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetAttributeValueByID")]
    [Authorize(Roles = "Admin")]

    [ProducesResponseType(typeof(AttributeValueDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByID(int id)
    {
        var comm = new GetByIDAttributeValue.GetByIDAttrValueQuery { Id = id };
        var result = await mediator.Send(comm);
        return Ok(result);
    }

    /// <summary>
    /// update attribute value
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPut("values/{id:int}")]
    [ValidationFilter]
    [Authorize(Roles = "Admin")]

    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> UpdateAttributeValue(int id, [FromQuery] string value)
    {
        var comm = new UpdateAttributeValueCommand(id, value);
        await mediator.Send(comm);
        return NoContent();
    }
}
