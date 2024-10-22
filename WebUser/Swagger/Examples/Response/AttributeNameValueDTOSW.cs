namespace WebUser.features.AttributeValue.DTO;

using Swashbuckle.AspNetCore.Filters;
using WebUser.features.AttributeName.DTO;

public class AttributeNameValueDTOSW : IExamplesProvider<AttributeNameValueDTO>
{
    public AttributeNameValueDTO GetExamples() =>
        new()
        {
            AttributeName = new AttributeNameMinDTO { Name = "color", ID = 1 },
            Attributes = new List<AttributeValueDTO>
            {
                new() { ID = 1, Value = "red" },
                new() { ID = 1, Value = "black" },
            },
        };
}
