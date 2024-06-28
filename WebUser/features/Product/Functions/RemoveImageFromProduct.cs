using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Image.Exceptions;
using WebUser.features.Product.Exceptions;

namespace WebUser.features.Product.Functions
{
    public class RemoveImageFromProduct
    {
        //input
        public class DeleteImageCommand : IRequest
        {
            public int ProductId { get; set; }
            public int ImageId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<DeleteImageCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(DeleteImageCommand request, CancellationToken cancellationToken)
            {
                var product =
                    await dbcontext
                        .Products.Include(q => q.Images)
                        .FirstOrDefaultAsync(q => q.ID == request.ImageId, cancellationToken: cancellationToken)
                    ?? throw new ProductNotFoundException(request.ProductId);
                var image =
                    await dbcontext.Img.FirstOrDefaultAsync(q => q.ID == request.ImageId, cancellationToken: cancellationToken)
                    ?? throw new ImageNotFoundException(request.ImageId);
                if (product.Images.Contains(image))
                {
                    product.Images.Remove(image);
                    await dbcontext.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
