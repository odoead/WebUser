using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public List<AttributeNameCategory> Attributes { get; set; } = new List<AttributeNameCategory>();

        [ForeignKey("ParentCategoryID")]
        public Category? ParentCategory { get; set; }
        public int? ParentCategoryID { get; set; }
        public List<Category> Subcategories { get; set; } = new List<Category>();
        public List<PromotionCategory> Promotions { get; set; } = new List<PromotionCategory>();
    }
}
