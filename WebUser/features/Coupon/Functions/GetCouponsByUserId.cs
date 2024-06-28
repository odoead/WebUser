using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.Exceptions;
using WebUser.features.Coupon.DTO;
using WebUser.features.Order.Exceptions;

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
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<CouponDTO>> Handle(GetCouponByUserIDQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Orders.AnyAsync(q => q.UserID == request.UserId, cancellationToken: cancellationToken))
                    throw new OrderNotFoundException(-1);
                // throw new userNotFoundException(request.UserId);
                var coupons =
                    await dbcontext.Coupons.Where(q => q.Order.UserID == request.UserId).ToListAsync(cancellationToken: cancellationToken)
                    ?? throw new CategoryNotFoundException(-1);
                var results = mapper.Map<ICollection<CouponDTO>>(coupons);
                return results;
            }
        }
    }
}
