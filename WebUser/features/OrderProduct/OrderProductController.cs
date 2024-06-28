namespace WebUser.features.OrderProduct;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.Order.DTO;
using WebUser.features.OrderProduct.DTO;
using WebUser.features.OrderProduct.Functions;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class OrderProductController : ControllerBase
{
    private readonly IMediator mediator;

    public OrderProductController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<OrderProductDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var query = new GetAllOrderProds.GetAllOrderProdsAsyncQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("order/{orderid:int}", Name = "GetOrderProductsByOrderID")]
    [ProducesResponseType(typeof(List<OrderProductDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByUserID(int orderid)
    {
        var query = new GetOrderProdByOrderID.GetByOrderProdIDQuery { OrderId = orderid };
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
