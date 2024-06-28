namespace WebUser.features.AttributeValue.DTO;

using WebUser.features.AttributeName.DTO;

public class AttributeNameValueDTO
{
    public List<AttributeValueDTO> Attributes { get; set; }
    public AttributeNameMinDTO AttributeName { get; set; }
}
