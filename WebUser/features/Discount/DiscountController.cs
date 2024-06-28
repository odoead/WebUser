namespace WebUser.features.Discount;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.Coupon.DTO;
using WebUser.features.discount.DTO;
using WebUser.features.discount.Functions;

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
    [ProducesResponseType(typeof(List<DiscountDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var query = new GetAllDiscounts.GetAllDiscountsQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("product/{id:int}", Name = "GetDiscountByProductID")]
    [ProducesResponseType(typeof(DiscountDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetDiscountByProductID(int id)
    {
        var query = new GetDiscountsByProductID.GetDiscountByProductIDQuery { ProductId = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
