namespace WebUser.features.Category;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Functions;
using WebUser.shared;
using WebUser.shared.Action_filter;
using WebUser.shared.RequestForming.features;

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
    [Authorize(Roles = "Admin")]
    [ValidationFilter]
    [ProducesResponseType(typeof(CategoryDTO), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create([FromBody] CreateCategory.CreateCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtRoute("GetCategoryByID", new { id = result.ID }, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteCategory.DeleteCategoryCommand { ID = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    [Paging]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedList<CategoryDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAll([FromQuery] CategoryRequestParameters parameters)
    {
        var query = new GetAllCategories.GetAllCategoryAsyncQuery { Parameters = parameters };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetCategoryByID")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CategoryDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetByID(int id)
    {
        var query = new GetCategoryByID.GetCategoryByIDQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("{id:int}/attribute/{attributeid:int}/add")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> AddAttribute(int id, int attributeid)
    {
        var command = new AddAttrNameToCategory.AddAttrNameToCategoryCommand { AttributeNameID = attributeid, CategoryId = id, };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}/attribute/{attributeid:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> RemoveAttribute(int id, int attributeid)
    {
        var command = new RemoveAttrNameFromCategory.RemoveAttrNameFromCategoryCommand { CategoryId = id, AttributeNameID = attributeid };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    [ValidationFilter]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> UpdateCategory(int id, [FromBody] JsonPatchDocument<UpdateCategoryDTO> patchDoc)
    {
        var command = new UpdateCategory.UpdateCategoryCommand { Id = id, PatchDoc = patchDoc };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{id:int}/children/first-gen")]
    [ProducesResponseType(typeof(ICollection<CategoryDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetFirstGenChildren(int id)
    {
        var query = new ShowFirstGenChildCategories.GetFirstGenChildCategoriesQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}/children")]
    [ProducesResponseType(typeof(ICollection<CategoryDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAllChildren(int id)
    {
        var query = new ShowAllChildCategories.GetAllChildCategoriesQuery { Id = id };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}/filters/catalog")]
    [ProducesResponseType(typeof(CategoryAttributeFiltersDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetCatalogFilters(int id, [FromQuery] bool includeChildCategories)
    {
        var query = new GetCategoryFiltersCatalog.GetCategoryFiltersCatalogQuery { Id = id, IncludeChildCategories = includeChildCategories };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}/filters/search")]
    [ProducesResponseType(typeof(SearchAttributeFiltersDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetSearchFilters(int id, [FromQuery] string productName)
    {
        var query = new GetSearchFiltesCatalog.GetSearchFiltesCatalogQuery { Id = id, ProductName = productName };
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
