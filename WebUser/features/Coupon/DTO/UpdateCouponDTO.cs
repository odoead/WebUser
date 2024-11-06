namespace WebUser.features.Coupon.DTO
{
    public class UpdateCouponDTO
    {
        public bool IsActivated { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public double? DiscountVal { get; set; }
        public float? DiscountPercent { get; set; }
    }
}
