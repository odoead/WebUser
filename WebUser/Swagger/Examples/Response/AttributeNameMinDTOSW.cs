namespace WebUser.features.AttributeName.DTO;

using Swashbuckle.AspNetCore.Filters;

public class AttributeNameMinDTOSW : IExamplesProvider<AttributeNameMinDTO>
{
    public AttributeNameMinDTO GetExamples() => new AttributeNameMinDTO { ID = 1, Name = "color" };
}
