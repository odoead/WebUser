namespace WebUser.features.OrderProduct;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


}
