using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.Coupon.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.Coupon.Functions
{
    public class GetAllCoupons
    {
        //input
        public class GetAllCouponAsyncQuery : IRequest<ICollection<CouponDTO>>
        {
            public RequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllCouponAsyncQuery, ICollection<CouponDTO>>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<CouponDTO>> Handle(GetAllCouponAsyncQuery request, CancellationToken cancellationToken)
            {
                var coupons = await dbcontext.Coupons.ToListAsync(cancellationToken: cancellationToken);
                var results = mapper.Map<ICollection<CouponDTO>>(coupons);
                return PagedList<CouponDTO>.PaginateList(results, results.Count, request.Parameters.PageNum, request.Parameters.PageSize);
            }
        }
    }
}
