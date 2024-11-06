using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.exceptions;
using WebUser.features.Coupon.DTO;
using WebUser.features.Order.Exceptions;
using WebUser.features.Product.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Coupon.Functions
{
    public class GetCouponsByOrderId
    {
        //input
        public class GetCouponByOrderIDQuery : IRequest<ICollection<CouponDTO>>
        {
            public int OrderId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetCouponByOrderIDQuery, ICollection<CouponDTO>>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<ICollection<CouponDTO>> Handle(GetCouponByOrderIDQuery request, CancellationToken cancellationToken)
            {
                if (!await dbcontext.Orders.AnyAsync(q => q.ID == request.OrderId, cancellationToken: cancellationToken))
                {
                    throw new OrderNotFoundException(request.OrderId);
                }

                var coupons =
                    await dbcontext.Coupons.Where(q => q.Order.ID == request.OrderId).ToListAsync(cancellationToken: cancellationToken)
                    ?? throw new RelatedEntityNotFoundException(nameof(E.Coupon), nameof(GetCouponsByOrderId), "Handle");

                var couponDTOs = new List<CouponDTO>();
                foreach (var coupon in coupons)
                {
                    var couponDTO = new CouponDTO
                    {
                        ID = coupon.ID,
                        ActiveFrom = coupon.ActiveFrom,
                        ActiveTo = coupon.ActiveTo,
                        Code = coupon.Code,
                        CreatedAt = coupon.CreatedAt,
                        DiscountPercent = coupon.DiscountPercent,
                        DiscountVal = coupon.DiscountVal,
                        IsActivated = coupon.IsActivated,
                        Product = new ProductMinDTO
                        {
                            ID = coupon.Product.ID,
                            Name = coupon.Product.Name,
                            Price = coupon.Product.Price,
                        },
                        IsActive = E.Coupon.IsActive(coupon),
                        OrderID = coupon.OrderID,
                    };
                    couponDTOs.Add(couponDTO);
                }

                return couponDTOs;
            }
        }
    }
}
