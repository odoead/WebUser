using WebUser.features.Product.DTO;

namespace WebUser.features.Coupon.DTO
{
    public class CouponDTO
    {
        public int ID { get; set; }
        public bool IsActivated { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public double? DiscountVal { get; set; }
        public float? DiscountPercent { get; set; }
        public int? OrderID { get; set; }
        public ProductMinDTO Product { get; set; }
    }
}
