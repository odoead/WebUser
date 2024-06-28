using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Image.DTO;
using WebUser.features.Image.Exceptions;
using WebUser.features.Product.Exceptions;

namespace WebUser.features.Image.Functions
{
    public class GetImagesByProductID
    {
        public class GetImagesByProductIDQuery : IRequest<List<ImageDTO>>
        {
            public int ProductId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetImagesByProductIDQuery, List<ImageDTO>>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<List<ImageDTO>> Handle(GetImagesByProductIDQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Products.AnyAsync(q => q.ID == request.ProductId, cancellationToken: cancellationToken))
                    throw new ProductNotFoundException(request.ProductId);
                var images =
                    await dbcontext.Img.Where(q => q.Product.ID == request.ProductId).ToListAsync(cancellationToken: cancellationToken)
                    ?? throw new ImageNotFoundException(-1);
                var results = mapper.Map<List<ImageDTO>>(images);
                return results;
            }
        }
    }
}
