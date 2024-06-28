using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Promotion.Exceptions;

namespace WebUser.features.Promotion.Functions
{
    public class DeletePromotion
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
                var promotion =
                    await dbcontext.Promotions.Where(q => q.ID == request.ID).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new PromotionNotFoundException(request.ID);
                dbcontext.Promotions.Remove(promotion);
                await dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
