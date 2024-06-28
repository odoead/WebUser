namespace WebUser.features.Order.DTO;

using WebUser.features.OrderProduct.DTO;

public class OrderUserDTO
{
    public int ID { get; set; }
    public string DeliveryAddress { get; set; }
    public int DeliveryMethod { get; set; }
    public int PaymentMethod { get; set; }
    public bool Status { get; set; }
    public double Payment { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? PointsUsed { get; set; } = null;
    public List<OrderProductDTO> OrderProducts { get; set; }
}
