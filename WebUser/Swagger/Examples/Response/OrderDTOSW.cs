using Swashbuckle.AspNetCore.Filters;
using WebUser.features.Coupon.DTO;
using WebUser.features.OrderProduct.DTO;
using WebUser.features.Point.DTO;
using WebUser.features.Product.DTO;

namespace WebUser.features.Order.DTO
{
    public class OrderDTOSW : IExamplesProvider<OrderDTO>
    {
        public OrderDTO GetExamples() =>
            new OrderDTO
            {
                ID = 1,
                DeliveryAddress = "street 6",
                DeliveryMethod = 1,
                PaymentMethod = 2,
                Status = true,
                Payment = 220,
                CreatedAt = DateTime.UtcNow,
                UserID = "userid",
                PointsUsed = 100,
                ActivatedCoupons = new List<CouponMinDTO>
                {
                    new CouponMinDTO
                    {
                        ID = 1,
                        DiscountVal = 10.0,
                        IsActive = true
                    },
                },
                ActivatedPoints = new List<PointMinDTO>
                {
                    new PointMinDTO
                    {
                        ID = 1,
                        Value = 100,
                        BalanceLeft = 0,
                        IsActive = true
                    }
                },
                OrderProducts = new List<OrderProductDTO>
                {
                    new OrderProductDTO
                    {
                        ID = 1,
                        Amount = 2,
                        Product = new ProductMinDTO { ID = 1, Name = "apple" },
                        FinalPrice = 20.0,
                        TotalFinalPrice = 40
                    },
                    new OrderProductDTO
                    {
                        ID = 2,
                        Amount = 10,
                        Product = new ProductMinDTO { ID = 2, Name = "banana" },
                        FinalPrice = 30,
                        TotalFinalPrice = 300
                    }
                }
            };
    }
}