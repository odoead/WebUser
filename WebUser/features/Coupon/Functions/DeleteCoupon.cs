using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Coupon.Exceptions;

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
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
            }

            public async Task Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
            {
                var coupon =
                    await dbcontext.Coupons.Where(q => q.ID == request.ID).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new CouponNotFoundException(request.ID);
                dbcontext.Coupons.Remove(coupon);
                await dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
