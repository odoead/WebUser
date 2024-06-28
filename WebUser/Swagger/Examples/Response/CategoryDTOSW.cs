using Swashbuckle.AspNetCore.Filters;
using WebUser.features.AttributeName.DTO;

namespace WebUser.features.Category.DTO
{
    public class CategoryDTOSW : IExamplesProvider<CategoryDTO>
    {
        public CategoryDTO GetExamples() =>
            new CategoryDTO
            {
                ID = 1,
                Name = "electronics",
                Attributes = new List<AttributeNameMinDTO>
                {
                    new AttributeNameMinDTO { Name = "color", ID = 4 },
                    new AttributeNameMinDTO { Name = "height", ID = 1 },
                    new AttributeNameMinDTO { Name = "width", ID = 2 }
                },
            };
    }
}
