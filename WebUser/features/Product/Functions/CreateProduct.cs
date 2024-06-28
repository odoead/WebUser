using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Product.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Product.Functions
{
    public class CreateProduct
    {
        //input
        public class CreateProductCommand : IRequest<ProductDTO>
        {
            public string Description { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public int Stock { get; set; }
            public ICollection<int> AttributeValuesID { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreateProductCommand, ProductDTO>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ProductDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                var attributeValues = await dbcontext
                    .ProductAttributeValues.Where(q => request.AttributeValuesID.Contains(q.AttributeValueID))
                    .ToListAsync(cancellationToken: cancellationToken);
                var product = new E.Product
                {
                    AttributeValues = attributeValues,
                    Description = request.Description,
                    Name = request.Name,
                    Price = request.Price,
                    Stock = request.Stock,
                    DateCreated = DateTime.UtcNow,
                    ReservedStock = 0
                };
                await dbcontext.Products.AddAsync(product, cancellationToken);
                await dbcontext.SaveChangesAsync(cancellationToken);
                var results = mapper.Map<ProductDTO>(product);
                return results;
            }
        }
    }
}
