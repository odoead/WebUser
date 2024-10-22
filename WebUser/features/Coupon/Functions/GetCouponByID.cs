using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.Exceptions;
using WebUser.features.Coupon.DTO;
using WebUser.features.Product.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Coupon.Functions
{
    public class GetCouponByID
    {
        //input
        public class GetCouponByIDQuery : IRequest<CouponDTO>
        {
            public int Id { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetCouponByIDQuery, CouponDTO>
        {

            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<CouponDTO> Handle(GetCouponByIDQuery request, CancellationToken cancellationToken)
            {
                var coupon =
                    await dbcontext.Coupons.FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken: cancellationToken)
                    ?? throw new CategoryNotFoundException(request.Id);
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

                return couponDTO;
            }
        }
    }
}
