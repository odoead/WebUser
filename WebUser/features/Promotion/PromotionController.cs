namespace WebUser.features.Promotion_TODO;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.Promotion.DTO;
using WebUser.features.Promotion.Functions;
using WebUser.features.Promotion_TODO.DTO;
using WebUser.features.Promotion_TODO.Functions;
using WebUser.shared;
using WebUser.shared.Action_filter;
using WebUser.shared.RequestForming.features;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class PromotionController : ControllerBase
{
    private readonly IMediator _mediator;

    public PromotionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ValidationFilter]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PromotionDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<PromotionDTO>> CreatePromotion([FromBody] CreatePromotion.CreatePromotionCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPromotionByID), new { id = result.ID }, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> DeletePromotion(int id)
    {
        var command = new DeletePromotion.DeleteCouponCommand { ID = id };
        await _mediator.Send(command);
        return NoContent();
    }

    // Get All Promotions with Pagination
    [HttpGet]
    [Paging]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedList<PromotionDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<PromotionDTO>>> GetAllPromotions([FromBody] PromotionRequestParameters parameters)
    {
        var query = new GetAllPromotions.GetAllPromotionsQuery(parameters);
        var result = await _mediator.Send(query);
        return Ok(result.ToList());
    }

    // Get Promotion by ID
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PromotionDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PromotionDTO>> GetPromotionByID(int id)
    {
        var query = new GetPromotionByID.GetByOrderProdIDQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // Update Promotion by ID using JsonPatch
    [HttpPatch("{id:int}")]
    [ValidationFilter]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> UpdatePromotion(int id, [FromBody] JsonPatchDocument<UpdatePromotionDTO> patchDoc)
    {
        var command = new UpdatePromotion.UpdatePromotionCommand { Id = id, PatchDoc = patchDoc };
        await _mediator.Send(command);
        return NoContent();
    }
}
