using Swashbuckle.AspNetCore.Filters;
using WebUser.features.AttributeName.DTO;

namespace WebUser.features.Category.DTO
{
    public class CategoryDTOSW : IExamplesProvider<CategoryDTO>
    {
        public CategoryDTO GetExamples() =>
            new()
            {
                ID = 1,
                Name = "electronics",
                Attributes = new List<AttributeNameMinDTO>
                {
                    new() { Name = "color", ID = 4 },
                    new() { Name = "height", ID = 1 },
                    new() { Name = "width", ID = 2 },
                },
            };
    }
}
