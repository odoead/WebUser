namespace WebUser.features.CartItem;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.Cart.DTO;
using WebUser.features.Cart.functions;
using WebUser.features.CartItem.DTO;
using WebUser.features.CartItem.Functions;
using WebUser.shared;
using static WebUser.features.CartItem.Functions.ChangeCartItemAmount;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CartItemController : ControllerBase
{
    private readonly IMediator mediator;

    public CartItemController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CartItemDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var query = new GetAllCarts.GetAllCartsQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("cart/{cartid:int}", Name = "GetCartItemsByCartID")]
    [ProducesResponseType(typeof(CartItemDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetCartItemsByCartID(int cartid)
    {
        var comm = new GetCartItemsByCartId.GetByCartIDQuery { CartId = cartid };
        var result = await mediator.Send(comm);
        return Ok(result);
    }

    [HttpPatch("{id:int}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> ChangeAmount(int id, [FromBody] int amount)
    {
        ChangeCartItemAmount.ChangeCartItemAmountCommand command = new ChangeCartItemAmountCommand { CartItemId = id, NewAmount = amount };
        await mediator.Send(command);
        return NoContent();
    }
}
