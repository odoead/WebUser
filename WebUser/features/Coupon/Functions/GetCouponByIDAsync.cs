using AutoMapper;
using MediatR;
using WebUser.features.Coupon.DTO;
using WebUser.features.Coupon.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.Coupon.Functions
{
    public class GetCouponByIDAsync
    {
        //input
        public class GetCouponByIDQuery : IRequest<CouponDTO>
        {
            public int Id { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetCouponByIDQuery, CouponDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<CouponDTO> Handle(GetCouponByIDQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Coupon.IsExistsAsync(new ObjectID<E.Coupon>(request.Id)))
                {
                    var categories = await _repoWrapper.Coupon.GetByIdAsync(new ObjectID<E.Coupon>(request.Id));
                    var results = _mapper.Map<CouponDTO>(categories);
                    return results;
                }
                throw new CouponNotFoundException(request.Id);
            }
        }
    }
}
