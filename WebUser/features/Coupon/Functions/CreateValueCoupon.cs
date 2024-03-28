using AutoMapper;
using AutoMapper;
using MediatR;
using E = WebUser.Domain.entities;
using WebUser.shared.RepoWrapper;
using WebUser.features.AttributeValue.DTO;
using System.ComponentModel.DataAnnotations;
using WebUser.features.Coupon.DTO;
using WebUser.shared;

namespace WebUser.features.Coupon.Functions
{
    public class CreateValueCoupon
    {
        //input
        public class CreateValueCouponCommand : IRequest<CouponDTO>
        {
            public DateTime ActiveFrom { get; set; }
            public DateTime ActiveTo { get; set; }
            public double DiscountVal { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<CreateValueCouponCommand, CouponDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<CouponDTO> Handle(CreateValueCouponCommand request, CancellationToken cancellationToken)
            {
                var coupon = new E.Coupon
                {
                    ActiveFrom = request.ActiveFrom,
                    ActiveTo = request.ActiveTo,
                    DiscountVal = request.DiscountVal,
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
