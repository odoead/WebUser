namespace WebUser.features.Discount;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.discount.DTO;
using WebUser.features.discount.Functions;
using WebUser.shared.Action_filter;
using WebUser.shared.RequestForming.features;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class DiscountController : ControllerBase
{
    private readonly IMediator mediator;

    public DiscountController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [Paging]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedList<DiscountDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll([FromQuery] DiscountRequestParameters parameters)
    {
        var query = new GetAllDiscounts.GetAllDiscountsQuery(parameters);
        var result = await mediator.Send(query);
        return Ok(
            new
            {
                Items = result.ToList(),

            });
    }

    [HttpGet("product/{id:int}", Name = "GetDiscountByProductID")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(DiscountDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetDiscountByProductID(int id)
    {
        var query = new GetDiscountsByProductID.GetDiscountByProductIDQuery { ProductId = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
