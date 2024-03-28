using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public int DeliveryMethod { get; set; }
        [Required]
        public int PaymentMethod { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public ICollection<Coupon> ActivatedCoupons { get; set; }
        public ICollection<OrderProduct> OrderProduct { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int? UserId { get; set; }
        public int? PointsUsed {  get; set; }
        public ICollection<Point> Points { get; set; }

    }
}
