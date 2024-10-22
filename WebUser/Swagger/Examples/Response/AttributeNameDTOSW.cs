using Swashbuckle.AspNetCore.Filters;
using WebUser.features.AttributeValue.DTO;

namespace WebUser.features.AttributeName.DTO
{
    public class AttributeNameDTOSW : IExamplesProvider<AttributeNameDTO>
    {
        public AttributeNameDTO GetExamples() =>
            new()
            {
                Name = "color",
                Description = "main color of the product",
                Id = 1,
                AttributeValues = new List<AttributeValueDTO>
                {
                    new() { ID = 1, Value = "black" },
                    new() { ID = 2, Value = "red" },
                    new() { ID = 1, Value = "blue" },
                },
            };
    }
}
