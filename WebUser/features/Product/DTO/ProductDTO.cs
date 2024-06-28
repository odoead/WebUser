using WebUser.features.AttributeValue.DTO;
using WebUser.features.Coupon.DTO;
using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;
using WebUser.features.Promotion_TODO.DTO;

namespace WebUser.features.Product.DTO
{
    public class ProductDTO
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int ReservedStock { get; set; }
        public bool IsPurchasable { get; set; }
        public DateTime DateCreated { get; set; }
        public List<ImageDTO> Images { get; set; }
        public List<AttributeNameValueDTO> AttributeValues { get; set; }
        public List<DiscountMinDTO> Discounts { get; set; }
        public List<CouponMinDTO> Coupons { get; set; }
        public List<PromotionMinDTO> Promotions { get; set; }
        //public ICollection<E.OrderProduct> OrderProduct { get; set; }
        //public ICollection<E.Product> PromotionProducts { get; set; }
        // public ICollection<E.CartItem> CartItems { get; set; }
    }
}
