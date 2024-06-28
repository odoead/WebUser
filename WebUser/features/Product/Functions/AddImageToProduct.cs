using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Image.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Product.Functions
{
    public class AddImageToProduct
    {
        //input
        public class AddImageToProductCommand : IRequest<ImageDTO>
        {
            public int ProductId { get; set; }
            public byte[] Image { get; set; }
            public IFormFile File { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<AddImageToProductCommand, ImageDTO>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ImageDTO> Handle(AddImageToProductCommand request, CancellationToken cancellationToken)
            {
                var product = await dbcontext.Products.FirstOrDefaultAsync(q => q.ID == request.ProductId, cancellationToken: cancellationToken);
                if (request.File.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await request.File.CopyToAsync(memoryStream, cancellationToken);
                        //based on the upload file to create Photo instance.
                        //You can also check the database, whether the image exists in the database.
                        var image = new E.Image() { ImageContent = memoryStream.ToArray(), Product = product, };
                        await dbcontext.Img.AddAsync(image, cancellationToken);
                        await dbcontext.SaveChangesAsync(cancellationToken);
                        var results = mapper.Map<ImageDTO>(image);
                        return results;
                    }
                }
                throw new NotImplementedException();
            }
        }
    }
}