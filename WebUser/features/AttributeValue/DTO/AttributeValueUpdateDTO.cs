using WebUser.features.AttributeName.DTO;

namespace WebUser.features.AttributeValue.DTO
{
    public class AttributeValueUpdateDTO
    {
        public string Value { get; set; }
        public AttributeNameDTO AttributeName { get; set; }
    }
}
