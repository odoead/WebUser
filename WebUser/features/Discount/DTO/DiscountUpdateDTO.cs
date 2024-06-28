using E = WebUser.Domain.entities;

namespace WebUser.features.discount.DTO
{
    public class DiscountUpdateDTO
    {
        public E.Product Product { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public double DiscountVal { get; set; }
        public float DiscountPercent { get; set; }
    }
}
