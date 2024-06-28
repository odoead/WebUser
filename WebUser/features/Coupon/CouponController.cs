namespace WebUser.features.Coupon;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.CartItem.DTO;
using WebUser.features.Coupon.DTO;
using WebUser.features.Coupon.Functions;
using WebUser.shared;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CouponController : ControllerBase
{
    private readonly IMediator mediator;

    public CouponController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(CouponDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Create([FromBody] CreateCoupon.CreateCouponCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetCouponByID", new { couponId = result.ID }, result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        var comm = new DeleteCoupon.DeleteCouponCommand { ID = id, };
        await mediator.Send(comm);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CouponDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var query = new GetAllCoupons.GetAllCouponAsyncQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetCouponByID")]
    [ProducesResponseType(typeof(CouponDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByID(int id)
    {
        var query = new GetCouponByID.GetCouponByIDQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("order/{orderid:int}", Name = "GetCouponsByOrderID")]
    [ProducesResponseType(typeof(CouponDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByOrderID(int orderid)
    {
        var query = new GetCouponsByOrderId.GetCouponByOrderIDQuery { OrderId = orderid };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("order/{string:int}", Name = "GetCouponsByUserID")]
    [ProducesResponseType(typeof(CouponDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByUserID(string userid)
    {
        var query = new GetCouponsByUserId.GetCouponByUserIDQuery { UserId = userid };
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
