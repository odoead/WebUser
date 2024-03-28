using E=WebUser.Domain.entities;

namespace WebUser.features.AttributeName.DTO
{
    public class AttributeNameUpdateDTO
    {
        
        public string Name { get; set; }
        public ICollection<E.AttributeName> Attributes { get; set; }
        public string Description { get; set; }
    }
}
