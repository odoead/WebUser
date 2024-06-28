namespace WebUser.features.Category;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.CartItem.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Functions;
using WebUser.shared;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CategoryController : ControllerBase
{
    private readonly IMediator mediator;

    public CategoryController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(CategoryDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create([FromBody] CreateCategory.CreateCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetCategoryByID", new { cartId = result.ID }, result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(CategoryDTO), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        var comm = new DeleteCategory.DeleteCategoryCommand { ID = id, };
        await mediator.Send(comm);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll()
    {
        var query = new GetAllCategories.GetAllCategoryAsyncQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetCategoryByID")]
    [ProducesResponseType(typeof(CategoryDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByID(int id)
    {
        var query = new GetCategoryByID.GetCategoryByIDQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPatch("{id:int}/attribute/{attributeid:int}/add")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> AddAttribute(int id, int attributeid)
    {
        var command = new AddAttrNameToCategory.AddAttrNameToCategoryCommand { AttributeNameID = id, CategoryId = attributeid };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("{id:int}/attribute/{attributeid:int}/remove")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> RemoveAttribute(int id, int attributeid)
    {
        var command = new RemoveAttrNameFromCategory.RemoveAttrNameFromCategoryCommand { CategoryId = id, AttributeNameID = attributeid, };
        await mediator.Send(command);
        return NoContent();
    }
}
