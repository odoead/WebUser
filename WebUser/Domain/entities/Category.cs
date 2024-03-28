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
        public ICollection<AttributeName> Attributes { get; set; }
        [ForeignKey("ParentCategoryId")]
        public Category? ParentCategory { get; set; }
        public int? ParentCategoryId { get; set; }
        public ICollection<Category> Subcategories { get; set; }
    }
}
