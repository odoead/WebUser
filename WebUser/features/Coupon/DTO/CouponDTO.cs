using System.ComponentModel.DataAnnotations;
using E=WebUser.Domain.entities;

namespace WebUser.features.Coupon.DTO
{
    public class CouponDTO
    {
        public int ID { get; set; }
        public bool IsActivated { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public double DiscountVal { get; set; }
        public float DiscountPercent { get; set; }
        public E.User? User { get; set; } = null;
        public E.Order? Order { get; set; } = null;
    }
}
