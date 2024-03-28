using WebUser.Domain.entities;
using E = WebUser.Domain.entities;

namespace WebUser.features.Order.DTO
{
    public class OrderDTO
    {
        public int ID { get; set; }
        public string DeliveryAddress { get; set; }
        public int DeliveryMethod { get; set; }
        public int PaymentMethod { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<E.Coupon>? ActivatedCoupons { get; set; } = null;
        public ICollection<E.OrderProduct>? OrderProduct { get; set; } = null;
        public User? User { get; set; } = null;
        public int? PointsUsed { get; set; }
        public ICollection<E.Point>? Points { get; set; } = null;
    }
}

