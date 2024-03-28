using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using E=WebUser.Domain.entities;
namespace WebUser.features.Category.DTO
{
    public class CategoryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<E.AttributeName>? Attributes { get; set; } = null;
        public E.Category? ParentCategory { get; set; } = null;
        public ICollection<E.Category>? Subcategories { get; set; } = null;
    }
}
