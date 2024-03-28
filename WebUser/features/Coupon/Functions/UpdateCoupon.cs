using AutoMapper;
using MediatR;
using E = WebUser.Domain.entities;
using WebUser.features.Category.DTO;
using WebUser.Domain.entities;
using WebUser.features.Category.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebUser.features.AttributeValue.DTO;
using WebUser.shared.RepoWrapper;
using WebUser.features.Coupon.DTO;
using WebUser.features.Coupon.Exceptions;
using WebUser.shared;

namespace WebUser.features.Coupon.Functions
{
    public class UpdateDiscount
    {
        //input
        public class UpdateCouponCommand : IRequest
        {
            public int ID { get; set; }
            public UpdateCouponDTO Coupon { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<UpdateCouponCommand>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {

                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Coupon.IsExistsAsync(new ObjectID<E.Coupon>(request.ID)))
                {
                    var coupon = await _repoWrapper.Coupon.GetByIdAsync(new ObjectID<E.Coupon>(request.ID));
                    _mapper.Map(request.Coupon, coupon);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new CouponNotFoundException(request.ID);
            }
        }

    }
}
