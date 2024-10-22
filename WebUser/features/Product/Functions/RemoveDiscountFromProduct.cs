using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.discount.Exceptions;
using WebUser.features.Product.Exceptions;

namespace WebUser.features.discount.Functions
{
    public class RemoveDiscountFromProduct
    {
        //input
        public class DeleteDiscountCommand : IRequest
        {
            public int ProductID { get; set; }
            public int DiscountID { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<DeleteDiscountCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
            {
                if (!await dbcontext.Products.AnyAsync(q => q.ID == request.ProductID, cancellationToken: cancellationToken))
                {
                    throw new ProductNotFoundException(request.ProductID);
                }

                var discount =
                    await dbcontext.Discounts.FirstOrDefaultAsync(
                        q => q.Product.ID == request.ProductID && q.ID == request.DiscountID,
                        cancellationToken: cancellationToken
                    ) ?? throw new DiscountNotFoundException(-1);
                dbcontext.Discounts.Remove(discount);
                await dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
