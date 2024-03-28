using AutoMapper;
using MediatR;
using WebUser.features.Coupon.DTO;
using WebUser.features.Coupon.Exceptions;
using WebUser.features.Order.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
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
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<CouponDTO>> Handle(GetCouponByOrderIDQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Order.IsExistsAsync(new ObjectID<E.Order>(request.OrderId)))
                {
                    try
                    {
                        var Coupons = await _repoWrapper.Coupon.GetByOrderIdAsync(new ObjectID<E.Order>(request.OrderId));
                        var results = _mapper.Map<ICollection<CouponDTO>>(Coupons);
                        return results;
                    }
                    catch (Exception ex)
                    {
                        throw new CouponNotFoundException(-1);
                    }

                }
                throw new OrderNotFoundException(request.OrderId);

            }
        }
    }
}
