using AutoMapper;
using MediatR;
using WebUser.features.Coupon.DTO;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Coupon.Functions
{
    public class GetAllCouponsAsync
    {
        //input
        public class GetAllCouponAsyncQuery : IRequest<ICollection<CouponDTO>> { }
        //handler
        public class Handler : IRequestHandler<GetAllCouponAsyncQuery, ICollection<CouponDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<CouponDTO>> Handle(GetAllCouponAsyncQuery request, CancellationToken cancellationToken)
            {
                var coupons = await _repoWrapper.Coupon.GetAllAsync();
                var results = _mapper.Map<ICollection<CouponDTO>>(coupons);
                return results;
            }
        }

    }
}
