namespace WebUser.features.AttributeValue.DTO;

using Swashbuckle.AspNetCore.Filters;
using WebUser.features.AttributeName.DTO;

public class AttributeNameValueDTOSW : IExamplesProvider<AttributeNameValueDTO>
{
    public AttributeNameValueDTO GetExamples() =>
        new AttributeNameValueDTO
        {
            AttributeName = new AttributeNameMinDTO { Name = "color", ID = 1 },
            Attributes = new List<AttributeValueDTO>
            {
                new AttributeValueDTO { ID = 1, Value = "red" },
                new AttributeValueDTO { ID = 1, Value = "black" },
            }
        };
}
