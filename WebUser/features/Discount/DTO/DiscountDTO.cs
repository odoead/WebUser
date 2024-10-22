using WebUser.features.Product.DTO;

namespace WebUser.features.discount.DTO
{
    public class DiscountDTO
    {
        public int ID { get; set; }
        public ProductMinDTO? Product { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public bool IsActive { get; set; }
        public double? DiscountVal { get; set; }
        public float? DiscountPercent { get; set; }
    }
}
