namespace WebUser.features.Order.DTO;

using WebUser.features.OrderProduct.DTO;

public class CreateOrderDTO
{
    public int ID { get; set; }
    public string DeliveryAddress { get; set; }
    public int DeliveryMethod { get; set; }
    public int PaymentMethod { get; set; }
    public bool Status { get; set; }
    public double Payment { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderProductDTO> OrderProducts { get; set; }
    public string UserID { get; set; }
    public int? PointsUsed { get; set; }
}
