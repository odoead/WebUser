namespace WebUser.features.Cart.functions;

using System.Net;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.Cart.DTO;
using WebUser.shared;
using WebUser.shared.Action_filter;
using WebUser.shared.RequestForming.features;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CartController : ControllerBase
{
    private readonly IMediator mediator;

    public CartController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("user/{userID}")]
    [Authorize(Roles = "Admin")]
    [ValidationFilter]
    [ProducesResponseType(typeof(CartDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create(string userID)
    {
        var command = new CreateCart.CreateCartCommand { UserId = userID };
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetCartByID", new { id = result.ID }, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        var comm = new DeleteCart.DeleteCartCommand { ID = id };
        await mediator.Send(comm);
        return NoContent();
    }

    [HttpGet]
    [Paging]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedList<CartDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll([FromQuery] CartRequestParameters request)
    {
        var query = new GetAllCarts.GetAllCartsQuery(request);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}/admin", Name = "GetCartByID")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CartDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByID(int id)
    {
        var query = new GetCartByID.GetByIDCartQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("user/{id}/admin", Name = "GetCartByUserID")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CartDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByUserId(string id)
    {
        var query = new GetCartByUserId.GetCartByUserIDQuery { UserId = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("user/{id}")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(typeof(PublicCartItemsDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserPublicCartItems([FromBody] GetUserPublicCartItems.GetUserPublicCartItemsQuery command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User notfound.");
        }

        command.UserId = userId;
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id:int}/product/{productid:int}")]
    [Authorize(Roles = "User")]
    [ValidationFilter]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> AddProduct(int id, int productid, [FromBody] int amount)
    {
        if (amount <= 0)
        {
            return BadRequest("Amount must be greater than 0.");
        }
        var command = new AddProductToCart.AddProductToCartCommand
        {
            Amount = amount,
            CartId = id,
            ProductId = productid,
        };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("{id:int}/product/{productid:int}")]
    [Authorize(Roles = "User")]
    [ValidationFilter]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> UpdateProduct(int id, int productid, [FromBody] int amount)
    {
        if (amount < 0)
        {
            return BadRequest("Amount cannot be negative.");
        }

        var command = new ChangeAmountRemoveProductFromCart.ChangeAmountRemoveProductFromCartCommand
        {
            CartId = id,
            ProductId = productid,
            NewAmount = amount,
        };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}/product/{productid:int}")]
    [ValidationFilter]
    [Authorize(Roles = "User")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> RemoveProduct(int id, int productid)
    {
        var command = new ChangeAmountRemoveProductFromCart.ChangeAmountRemoveProductFromCartCommand
        {
            NewAmount = 0,
            CartId = id,
            ProductId = productid,
        };
        await mediator.Send(command);
        return NoContent();
    }
}
