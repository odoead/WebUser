using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Coupon.DTO;
using WebUser.features.Coupon.Exceptions;
using WebUser.features.Order.Exceptions;

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
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<CouponDTO>> Handle(GetCouponByOrderIDQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Orders.AnyAsync(q => q.ID == request.OrderId, cancellationToken: cancellationToken))
                    throw new OrderNotFoundException(request.OrderId);
                var coupons =
                    await dbcontext.Coupons.Where(q => q.Order.ID == request.OrderId).ToListAsync(cancellationToken: cancellationToken)
                    ?? throw new CouponNotFoundException(-1);
                var results = mapper.Map<ICollection<CouponDTO>>(coupons);
                return results;
            }
        }
    }
}
