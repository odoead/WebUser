using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.exceptions;
using WebUser.features.Image.DTO;
using WebUser.features.Product.Exceptions;

namespace WebUser.features.Image.Functions
{
    public class GetImagesByProductID
    {
        public class GetImagesByProductIDQuery : IRequest<ICollection<ImageDTO>>
        {
            public int ProductId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetImagesByProductIDQuery, ICollection<ImageDTO>>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<ICollection<ImageDTO>> Handle(GetImagesByProductIDQuery request, CancellationToken cancellationToken)
            {
                if (!await dbcontext.Products.AnyAsync(q => q.ID == request.ProductId, cancellationToken: cancellationToken))
                {
                    throw new ProductNotFoundException(request.ProductId);
                }

                var images =
                    await dbcontext.Img.Where(q => q.Product.ID == request.ProductId).ToListAsync(cancellationToken: cancellationToken)
                    ?? throw new RelatedEntityNotFoundException(nameof(Image), nameof(GetImagesByProductID), "Handle");
                var results = new List<ImageDTO>();
                images.ForEach(image =>
                {
                    results.Add(new ImageDTO { ID = image.ID, ImageContent = image.ImageContent });
                });
                return results;
            }
        }
    }
}
