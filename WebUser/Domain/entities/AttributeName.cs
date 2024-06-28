using System.ComponentModel.DataAnnotations;

namespace WebUser.Domain.entities
{
    public class AttributeName
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Attribute name is required")]
        public string Name { get; set; }
        public ICollection<AttributeValue> AttributeValues { get; set; }
        public string? Description { get; set; }
        public ICollection<AttributeNameCategory> Categories { get; set; }
    }
}
