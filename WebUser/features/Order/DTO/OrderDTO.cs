using WebUser.features.OrderProduct.DTO;
using WebUser.features.Point.DTO;

namespace WebUser.features.Order.DTO
{
    public class OrderDTO
    {
        public int ID { get; set; }
        public string DeliveryAddress { get; set; }
        public int DeliveryMethod { get; set; }
        public int PaymentMethod { get; set; }
        public bool Status { get; set; }
        public double Payment { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PointMinDTO>? ActivatedPoints { get; set; } = new List<PointMinDTO>();
        public List<OrderProductDTO> OrderProducts { get; set; }
        public string UserID { get; set; }
        public int? PointsUsed { get; set; }
    }
}
