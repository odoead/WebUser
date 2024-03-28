using MediatR;
using WebUser.features.Coupon.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.Coupon.Functions
{
    public class DeleteCoupon
    {
        //input
        public class DeleteCouponCommand : IRequest
        {
            public int ID { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<DeleteCouponCommand>
        {
            private IRepoWrapper _repoWrapper;

            public Handler(IRepoWrapper ServiceWrapper)
            {
                _repoWrapper = ServiceWrapper;
            }

            public async Task Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
            {

                if (await _repoWrapper.Coupon.IsExistsAsync(new ObjectID<E.Coupon>(request.ID)))
                {
                    var Coupon = await _repoWrapper.Coupon.GetByIdAsync(new ObjectID<E.Coupon>(request.ID));
                    _repoWrapper.Coupon.Delete(Coupon);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new CouponNotFoundException(request.ID);
            }
        }

    }
}
