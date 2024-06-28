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

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int PointsUsed { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Payment { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
        public string UserID { get; set; }
        public ICollection<Coupon> ActivatedCoupons { get; set; }
        public ICollection<Point> Points { get; set; }
        public ICollection<OrderProduct> OrderProduct { get; set; }
    }
}
