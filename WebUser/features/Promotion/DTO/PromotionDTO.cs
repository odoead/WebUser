using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using E=WebUser.Domain.entities;
using WebUser.Domain.entities;

namespace WebUser.features.Promotion.DTO
{
    public class PromotionDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public E.Category Category { get; set; }
        public ICollection<E.AttributeName> AttributeNames { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public double DiscountVal { get; set; }
        public float DiscountPercent { get; set; }
    }
}
