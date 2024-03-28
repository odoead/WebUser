using E = WebUser.Domain.entities;

namespace WebUser.features.OrderProduct.DTO
{
    public class OrderProductDTO
    {
        public int ID { get; set; }
        public int Amount { get; set; }
        public E.Product? Product { get; set; } = null;
        public E.Order? Order { get; set; } = null;
    }
}
