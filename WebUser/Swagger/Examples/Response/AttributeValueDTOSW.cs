using Swashbuckle.AspNetCore.Filters;

namespace WebUser.features.AttributeValue.DTO
{
    public class AttributeValueDTOSW : IExamplesProvider<AttributeValueDTO>
    {
        public AttributeValueDTO GetExamples() => new AttributeValueDTO { Value = "red", ID = 1 };
    }
}
