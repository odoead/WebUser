using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using E=WebUser.Domain.entities;

namespace WebUser.features.discount.DTO
{
    public class DiscountDTO
    {
        public int ID { get; set; }
        public E.Product? Product { get; set; } = null;
        public DateTime CreatedAt { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public double DiscountVal { get; set; }
        public float DiscountPercent { get; set; }
    }
}
