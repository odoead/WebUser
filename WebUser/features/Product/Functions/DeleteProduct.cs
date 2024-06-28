using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Product.Exceptions;

namespace WebUser.features.Product.Functions
{
    public class DeleteProduct
    {
        //input
        public class DeleteProductCommand : IRequest
        {
            public int ID { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<DeleteProductCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var item =
                    await dbcontext.Products.Where(q => q.ID == request.ID).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new ProductNotFoundException(request.ID);
                dbcontext.Products.Remove(item);
                await dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
