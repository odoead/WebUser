namespace WebUser.features.Product;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.discount.Functions;
using WebUser.features.Image.DTO;
using WebUser.features.Point.DTO;
using WebUser.features.Point.Functions;
using WebUser.features.Product.Functions;
using WebUser.shared;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ProductController : ControllerBase
{
    private readonly IMediator mediator;

    public ProductController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(PointDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create([FromBody] CreateProduct.CreateProductCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetProductByID", new { productId = result.ID }, result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(PointDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Delete(int id)
    {
        var comm = new DeletePoint.DeletePointCommand { ID = id, };
        await mediator.Send(comm);
        return NoContent();
    }

    [HttpPatch("{id:int}/discount/add")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(PointDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> AddDiscount(int id, [FromBody] AddDiscountToProduct.AddDiscountToProductCommand command)
    {
        command.ProductId = id;
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("{id:int}/image/add")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(ImageDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> AddImage(int id, [FromBody] AddImageToProduct.AddImageToProductCommand command)
    {
        command.ProductId = id;
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("{id:int}/discount{disountid:int}/remove")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> RemoveDiscount(int id, int disountid)
    {
        var command = new RemoveDiscountFromProduct.DeleteDiscountCommand { ProductID = id, DiscountID = disountid };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("{id:int}/image{imageid:int}/remove")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> RemoveImage(int id, int imageid)
    {
        var command = new RemoveImageFromProduct.DeleteImageCommand { ProductId = id, ImageId = imageid };
        command.ProductId = id;
        await mediator.Send(command);
        return NoContent();
    }
}
