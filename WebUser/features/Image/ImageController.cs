namespace WebUser.features.Image;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.Coupon.DTO;
using WebUser.features.Image.DTO;
using WebUser.features.Image.Functions;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ImageController : ControllerBase
{
    private readonly IMediator mediator;

    public ImageController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("product/{id:int}", Name = "GetImageByProductID")]
    [ProducesResponseType(typeof(List<ImageDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetImagesByProductID(int id)
    {
        var query = new GetImagesByProductID.GetImagesByProductIDQuery { ProductId = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
