namespace WebUser.features.Product.Functions;

using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Product.DTO;
using WebUser.features.Product.Exceptions;

public class GetProductByID
{
    //input
    public class GetProductByIDRequest : IRequest<ProductDTO>
    {
        public int Id { get; set; }
    }

    //handler
    public class Handler : IRequestHandler<GetProductByIDRequest, ProductDTO>
    {
        private readonly IMapper mapper;
        private readonly DB_Context dbcontext;

        public Handler(IMapper mapper, DB_Context context)
        {
            this.mapper = mapper;
            dbcontext = context;
        }

        public async Task<ProductDTO> Handle(GetProductByIDRequest request, CancellationToken cancellationToken)
        {
            var data = await dbcontext.Products.FirstOrDefaultAsync(q => q.ID == request.Id) ?? throw new ProductNotFoundException(request.Id);
            var res = mapper.Map<ProductDTO>(data);
            return res;
        }
    }
}
