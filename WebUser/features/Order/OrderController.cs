namespace WebUser.features.Order;

using System.Net;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.Order.DTO;
using WebUser.features.Order.Functions;
using WebUser.shared;
using WebUser.shared.Action_filter;
using WebUser.shared.RequestForming.features;
using static WebUser.features.Order.Functions.CompleteOrder;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class OrderController : ControllerBase
{
    private readonly IMediator mediator;

    public OrderController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [ValidationFilter]
    [ProducesResponseType(typeof(OrderDTO), (int)HttpStatusCode.Created)]
    [Authorize]
    public async Task<ActionResult> Create([FromBody] CreateOrder.CreateOrderCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User notfound.");
        }
        command.UserId = userId;

        var result = await mediator.Send(command);
        return CreatedAtRoute("GetCouponByID", new { couponId = result.ID }, result);
    }

    [HttpPost("{id:int}")]
    [Authorize]
    public async Task<ActionResult> Complete(int id)
    {
        var command = new CompleteOrderCommand { Id = id };
        await mediator.Send(command);
        return NoContent();
    }





    [HttpGet]
    [Paging]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedList<OrderDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll([FromQuery] OrderRequestParameters parameters)
    {
        var query = new GetAllOrders.GetAllOrdersAsyncQuery(parameters);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetOrderByID")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(OrderDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByID(int id)
    {
        var query = new GetOrderByID.GetByIDQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("user/{userid}", Name = "GetOrdersByUserID")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByUserID(string userid)
    {
        var query = new GetOrdersByUser.GetOrdersByUserQuery { UserId = userid };
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
