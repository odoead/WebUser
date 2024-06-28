using WebUser.Data;
using WebUser.features.AttributeName;
using WebUser.features.AttributeName.Interfaces;
using WebUser.features.AttributeValue;
using WebUser.features.AttributeValue.Interfaces;
using WebUser.features.Cart;
using WebUser.features.Cart.Interfaces;
using WebUser.features.Category.Interfaces;
using WebUser.features.Coupon;
using WebUser.features.Coupon.Interfaces;
using WebUser.features.discount;
using WebUser.features.discount.Interfaces;
using WebUser.features.Image;
using WebUser.features.Image.Interfaces;
using WebUser.features.Order;
using WebUser.features.Order.Interfaces;
using WebUser.features.OrderProduct;
using WebUser.features.OrderProduct.Interfaces;
using WebUser.features.Point;
using WebUser.features.Point.Interfaces;
using WebUser.features.Promotion;
using WebUser.features.Promotion.Interfaces;
using WebUser.features.User;
using WebUser.features.User.Interfaces;
using WebUser.shared.RepoWrapper;

namespace WebUser.shared.ServiceWrapper
{
    public class ServiceWrapper : IServiceWrapper
    {
        public ServiceWrapper(DB_Context db)
        {
            this.db = db;
        }

        private readonly DB_Context db;
        private IAttributeNameService attributeNameServ;
        private IAttributeValueService attributeValueServ;
        private ICartService cartServ;
        private ICartItemService cartItemServ;
        private ICategoryService categoryServ;
        private ICouponService couponServ;
        private IDiscountService discountServ;
        private IImageService imageServ;
        private IOrderService orderServ;
        private IOrderProductService orderProductServ;
        private IPointService pointServ;
        private IProductService productServ;
        private IPromotionService promotionServ;
        private IUserService userServ;
        public IAttributeNameService AttributeName
        {
            get
            {
                if (attributeNameServ == null)
                    attributeNameServ = new AttributeNameService(db);
                return attributeNameServ;
            }
        }
        public IAttributeValueService AttributeValue
        {
            get
            {
                if (attributeValueServ == null)
                    attributeValueServ = new AttributeValueService(db);
                return attributeValueServ;
            }
        }
        public ICartService Cart
        {
            get
            {
                if (cartServ == null)
                    cartServ = new CartService(db);
                return cartServ;
            }
        }
        public ICartItemService CartItem
        {
            get
            {
                if (cartItemServ == null)
                    cartItemServ = new CartItemService(db);
                return cartItemServ;
            }
        }
        public ICategoryService Category
        {
            get
            {
                if (categoryServ == null)
                    categoryServ = new CategoryService(db);
                return categoryServ;
            }
        }
        public ICouponService Coupon
        {
            get
            {
                if (couponServ == null)
                    couponServ = new CouponService(db);
                return couponServ;
            }
        }
        public IDiscountService Discount
        {
            get
            {
                if (discountServ == null)
                    discountServ = new DiscountService(db);
                return discountServ;
            }
        }
        public IImageService Image
        {
            get
            {
                if (imageServ == null)
                    imageServ = new ImageService(db);
                return imageServ;
            }
        }
        public IOrderService Order
        {
            get
            {
                if (orderServ == null)
                    orderServ = new OrderService(db);
                return orderServ;
            }
        }
        public IOrderProductService OrderProduct
        {
            get
            {
                if (orderProductServ == null)
                    orderProductServ = new OrderProductService(db);
                return orderProductServ;
            }
        }
        public IPointService Point
        {
            get
            {
                if (pointServ == null)
                    pointServ = new PointService(db);
                return pointServ;
            }
        }
        public IProductService Product
        {
            get
            {
                if (productServ == null)
                    productServ = new ProductService(db);
                return productServ;
            }
        }
        public IPromotionService Promotion
        {
            get
            {
                if (promotionServ == null)
                    promotionServ = new PromotionService(db);
                return promotionServ;
            }
        }
        public IUserService User
        {
            get
            {
                //if (userServ == null)
                    //userServ = new UserService(db);
                return userServ;
            }
        }

        public Task SaveAsync() => db.SaveChangesAsync();
    }
}