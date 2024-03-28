using AutoMapper;
using MediatR;
using E=WebUser.Domain.entities;
using WebUser.features.Coupon.DTO;
using WebUser.features.Coupon.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Coupon.Functions
{
    public class GetCouponsByUserId
    {
        //input
        public class GetCouponByUserIDQuery : IRequest<ICollection<CouponDTO>>
        {
            public int UserId { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetCouponByUserIDQuery, ICollection<CouponDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<CouponDTO>> Handle(GetCouponByUserIDQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.user.IsExistsAsync(new ObjectID<E.User>(request.UserId)))
                {
                    try
                    {
                        var Coupons = await _repoWrapper.Coupon.GetByOrderIdAsync(new ObjectID<E.User>(request.UserId));
                        var results = _mapper.Map<ICollection<CouponDTO>>(Coupons);
                        return results;
                    }
                    catch (Exception ex)
                    {
                        throw new CouponNotFoundException(-1);
                    }

                }
                throw new Usernotfoundexception
            }
        }
    }
}
