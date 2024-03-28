using AutoMapper;
using MediatR;
using WebUser.features.Coupon.DTO;
using WebUser.shared.RepoWrapper;
using WebUser.shared;
using E=WebUser.Domain.entities;

namespace WebUser.features.Coupon.Functions
{
    public class CreatePercentCoupon
    {
        //input
        public class CreatePercentCouponCommand : IRequest<CouponDTO>
        {
            public DateTime ActiveFrom { get; set; }
            public DateTime ActiveTo { get; set; }
            public float DiscountPercent { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<CreatePercentCouponCommand, CouponDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<CouponDTO> Handle(CreatePercentCouponCommand request, CancellationToken cancellationToken)
            {
                var coupon = new E.Coupon
                {
                    ActiveFrom = request.ActiveFrom,
                    ActiveTo = request.ActiveTo,
                    DiscountPercent = request.DiscountPercent,
                    Code = CodeGenerator.GenerateCode(5),
                    CreatedAt = DateTime.Now,
                    IsActivated = false,

                };
                _repoWrapper.Coupon.CreateWithNumberDiscount(coupon);
                await _repoWrapper.SaveAsync();
                var results = _mapper.Map<CouponDTO>(coupon);
                return results;
            }
        }
    }
}
