using Microsoft.EntityFrameworkCore;
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

namespace WebUser.shared.RepoWrapper
{
    public class RepoWrapper : IRepoWrapper
    {
        public RepoWrapper(DB_Context db)
        {
            _db = db;
        }
        private DB_Context _db;
        private IAttributeNameService _attributeNameServ;
        private IAttributeValueService _attributeValueServ;
        private ICartService _cartServ;
        private ICartItemService _cartItemServ;
        private ICategoryService _categoryServ;
        private ICouponService _couponServ;
        private IDiscountService _discountServ;
        private IImageService _imageServ;
        private IOrderService _orderServ;
        private IOrderProductService _orderProductServ;
        private IPointService _pointServ;
        private IProductService _productServ;
        private IPromotionService _promotionServ;
        public IAttributeNameService AttributeName 
        {
            get
            {
                if (_attributeNameServ == null)
                    _attributeNameServ = new AttributeNameService(_db);

                return _attributeNameServ;
            }
        }

        public IAttributeValueService AttributeValue
        {
            get
            {
                if (_attributeValueServ == null)
                    _attributeValueServ = new AttributeValueRepo(_db);

                return _attributeValueServ;
            }
        }

        public ICartService Cart
        {
            get
            {
                if (_cartServ == null)
                    _cartServ = new CartRepo(_db);

                return _cartServ;
            }
        }

        public ICartItemService CartItem
        {
            get
            {
                if (_cartItemServ == null)
                    _cartItemServ = new CartItemRepo(_db);

                return _cartItemServ;
            }
        }

        public ICategoryService Category
        {
            get
            {
                if (_categoryServ == null)
                    _categoryServ = new CategoryRepo(_db);

                return _categoryServ;
            }
        }

        public ICouponService Coupon
        {
            get
            {
                if (_couponServ == null)
                    _couponServ = new CouponRepo(_db);

                return _couponServ;
            }
        }

        public IDiscountService Discount
        {
            get
            {
                if (_discountServ == null)
                    _discountServ = new DiscountRepo(_db);

                return _discountServ;
            }
        }

        public IImageService Image
        {
            get
            {
                if (_imageServ == null)
                    _imageServ = new ImageRepo(_db);

                return _imageServ;
            }
        }

        public IOrderService Order
        {
            get
            {
                if (_orderServ == null)
                    _orderServ = new OrderRepo(_db);

                return _orderServ;
            }
        }

        public IOrderProductService OrderProduct
        {
            get
            {
                if (_orderProductServ == null)
                    _orderProductServ = new OrderProductRepo(_db);

                return _orderProductServ;
            }
        }

        public IPointService Point
        {
            get
            {
                if (_pointServ == null)
                    _pointServ = new PointRepo(_db);

                return _pointServ;
            }
        }

        public IProductService Product
        {
            get
            {
                if (_productServ == null)
                    _productServ = new ProductRepo(_db);

                return _productServ;
            }
        }

        public IPromotionService Promotion
        {
            get
            {
                if (_promotionServ == null)
                    _promotionServ = new PromotionRepo(_db);

                return _promotionServ;
            }
        }
        public Task SaveAsync()
        {
            return  _db.SaveChangesAsync();
        }
    }
}
