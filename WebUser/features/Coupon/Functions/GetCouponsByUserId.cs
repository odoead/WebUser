using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.Exceptions;
using WebUser.features.Coupon.DTO;
using WebUser.features.Order.Exceptions;
using WebUser.features.Product.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Coupon.Functions
{
    public class GetCouponsByUserId
    {
        //input
        public class GetCouponByUserIDQuery : IRequest<ICollection<CouponDTO>>
        {
            public string UserId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetCouponByUserIDQuery, ICollection<CouponDTO>>
        {

            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<ICollection<CouponDTO>> Handle(GetCouponByUserIDQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Orders.AnyAsync(q => q.UserID == request.UserId, cancellationToken: cancellationToken))
                {
                    throw new OrderNotFoundException(-1);
                }

                var coupons =
                    await dbcontext
                        .Coupons.Include(q => q.Order)
                        .Where(q => q.Order.UserID == request.UserId)
                        .ToListAsync(cancellationToken: cancellationToken) ?? throw new CategoryNotFoundException(-1);

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
