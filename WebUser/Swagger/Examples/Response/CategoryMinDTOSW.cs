namespace WebUser.features.Category.DTO;

using Swashbuckle.AspNetCore.Filters;

public class CategoryMinDTOSW : IExamplesProvider<CategoryMinDTO>
{
    public CategoryMinDTO GetExamples() => new CategoryMinDTO { ID = 4, Name = "color" };
}
