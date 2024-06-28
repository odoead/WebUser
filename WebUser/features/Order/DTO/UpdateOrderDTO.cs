using E = WebUser.Domain.entities;

namespace WebUser.features.Order.DTO
{
    public class UpdateOrderDTO
    {
        public List<E.OrderProduct> products { get; set; }
        public E.User User { get; set; }
        public string DeliveryAddress { get; set; }
        public int DeliveryMethod { get; set; }
        public int PaymentMethod { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
