namespace WebUser.features.Product;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.discount.DTO;
using WebUser.features.discount.Functions;
using WebUser.features.Image.DTO;
using WebUser.features.Product.DTO;
using WebUser.features.Product.extensions;
using WebUser.features.Product.Functions;
using WebUser.shared;
using WebUser.shared.Action_filter;
using WebUser.shared.RequestForming.features;
using static WebUser.features.Product.Functions.ChangeAmountDeleteProduct;

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
    [ValidationFilter]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create([FromBody] CreateProduct.CreateProductCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetProductByID", new { productId = result.ID }, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> ChangeProductAmount(int id, [FromBody] int amount)
    {
        var command = new DeleteProductCommand { ID = id, NewAmount = amount };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id}/discount")]
    [ValidationFilter]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(DiscountDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> AddDiscount(int id, [FromBody] AddDiscountToProduct.AddDiscountToProductCommand command)
    {
        command.ProductId = id;
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetDiscountByID", new { id = result.ID }, result);
    }

    [HttpPost("{id}/review")]
    [ValidationFilter]
    [Authorize]
    [ProducesResponseType(typeof(DiscountDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> AddReview(int id, [FromBody] AddReviewToProduct.AddReviewToProductCommand command)
    {
        command.ProductID = id;
        var result = await mediator.Send(command);
        return Ok(result);
        //return CreatedAtRoute("GetReviewByID", new { id = result.ID }, result);
    }

    [HttpPost("{id}/image")]
    [ValidationFilter]
    [Authorize]
    [ProducesResponseType(typeof(ImageDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> AddImage(int id, [FromBody] AddImageToProduct.AddImageToProductCommand command)
    {
        command.ProductId = id;
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:int}/discount/{disountid:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> RemoveDiscount(int id, int disountid)
    {
        var command = new RemoveDiscountFromProduct.DeleteDiscountCommand { ProductID = id, DiscountID = disountid };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}/image/{imageid:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> RemoveImage(int id, int imageid)
    {
        var command = new RemoveImageFromProduct.DeleteImageCommand { ProductId = id, ImageId = imageid };
        command.ProductId = id;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet()]
    [Paging]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedList<ProductDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAllProducts([FromQuery] ProductRequestParameters parameters)
    {
        var query = new GetAllProducts.GetAllProductsQuery { Parameters = parameters };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("thumbnails")]
    [Paging]
    [ProducesResponseType(typeof(PagedList<ProductThumbnailDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAllThumbnailProducts(
        [FromQuery] ProductRequestParameters parameters,
        int? categoryId = 0,
        bool includeChildCategories = false
    )
    {
        var query = new GetAllProductsThumbnail.GetAllProductsThumbnailQuery
        {
            Parameters = parameters,
            CategoryId = categoryId,
            IncludeChildCategories = includeChildCategories,
        };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetProductById(int id)
    {
        var query = new GetProductByID.GetProductByIDQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}/productpage")]
    [ProducesResponseType(typeof(ProductPageDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetProductPageById(int id)
    {
        var query = new GetProductPage.GetProductPageByIDQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody] JsonPatchDocument<UpdateProductDTO> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest("Patch document is null.");
        }
        var command = new UpdateProduct.UpdateProductCommand { Id = id, PatchDoc = patchDoc };
        await mediator.Send(command);
        return NoContent();
    }
}
