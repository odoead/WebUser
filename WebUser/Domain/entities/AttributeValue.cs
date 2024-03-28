using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class AttributeValue
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Attribute value is required")]
        public string Value { get; set; }
        [ForeignKey("AttributeNameID")]
        public AttributeName? AttributeName { get; set; }
        public int? AttributeNameID { get; set; }
        [ForeignKey("ProductID")]
        public Product? Product { get; set; }
        public int? ProductID { get; set; }
    }
}
