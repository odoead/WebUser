using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.Exceptions;
using WebUser.features.Coupon.DTO;

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
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<CouponDTO> Handle(GetCouponByIDQuery request, CancellationToken cancellationToken)
            {
                var coupon =
                    await dbcontext.Coupons.FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken: cancellationToken)
                    ?? throw new CategoryNotFoundException(request.Id);
                var results = mapper.Map<CouponDTO>(coupon);
                return results;
            }
        }
    }
}
