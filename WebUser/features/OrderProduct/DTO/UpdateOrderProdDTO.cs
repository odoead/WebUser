using E = WebUser.Domain.entities;

namespace WebUser.features.OrderProduct.DTO
{
    public class UpdateOrderProdDTO
    {
        public E.Order Order { get; set; }
        public int Amount { get; set; }
    }
}
