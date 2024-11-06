using WebUser.features.AttributeValue.DTO;

namespace WebUser.features.AttributeName.DTO
{
    public class AttributeNameUpdateDTO
    {
        public string Name { get; set; }
        public List<AttributeValueDTO> Attributes { get; set; } = new List<AttributeValueDTO>();
        public string Description { get; set; }
    }
}
