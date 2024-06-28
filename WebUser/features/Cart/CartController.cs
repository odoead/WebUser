namespace WebUser.features.Cart.functions;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Cart.DTO;
using WebUser.shared;

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

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(CartDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create([FromBody] CreateCart.CreateCartCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetCartByID", new { cartId = result.ID }, result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        var comm = new DeleteCart.DeleteCartCommand { ID = id };
        await mediator.Send(comm);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CartDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var query = new GetAllCarts.GetAllCartsQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetCartByID")]
    [ProducesResponseType(typeof(CartDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByID(int id)
    {
        var query = new GetCartByID.GetByIDCartQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("user/{id}", Name = "GetCartByUserID")]
    [ProducesResponseType(typeof(CartDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByUserId(string id)
    {
        var query = new GetCartByUserId.GetCartByUserIDQuery { UserId = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPatch("{id:int}/product/{productid:int}/add")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> AddProduct(int id, int productid, [FromBody] int amount)
    {
        var command = new AddProductsToCart.AddProductToCartCommand
        {
            Amount = amount,
            CartId = id,
            ProductId = productid
        };
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id">test</param>
    /// <param name="productid"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}/product/{productid:int}/remove")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> RemoveProduct(int id, int productid)
    {
        var command = new RemoveProductFromCart.RemoveProductFromCartCommand { CartId = id, ProductId = productid, };
        await mediator.Send(command);
        return NoContent();
    }
}
