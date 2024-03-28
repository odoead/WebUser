using E=WebUser.Domain.entities;

namespace WebUser.features.AttributeValue.DTO
{
    public class AttributeValueUpdateDTO
    {
        public string Value { get; set; }
        public E.AttributeName? AttributeName { get; set; }
    }
}
