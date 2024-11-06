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
        public AttributeName AttributeName { get; set; }
        public int AttributeNameID { get; set; }
        public List<ProductAttributeValue> Products { get; set; } = new List<ProductAttributeValue>();
        public List<PromotionAttrValue> Promotions { get; set; } = new List<PromotionAttrValue>();
    }
}
