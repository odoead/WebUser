namespace WebUser.features.Order;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.Image.DTO;
using WebUser.features.Order.DTO;
using WebUser.features.Order.Functions;
using WebUser.shared;

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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(OrderDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create([FromBody] CreateOrder.CreateOrderCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetCouponByID", new { couponId = result.ID }, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var query = new GetAllOrders.GetAllOrdersAsyncQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetOrderByID")]
    [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByID(int id)
    {
        var query = new GetOrderByID.GetByIDQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("order/{string:int}", Name = "GetOrderByUserID")]
    [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByUserID(string userid)
    {
        var query = new GetOrdersByUser.GetOrdersByUserQuery { UserId = userid };
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
