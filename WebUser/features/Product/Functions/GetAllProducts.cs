namespace WebUser.features.Product.Functions;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.DTO;
using WebUser.features.Product.DTO;
using WebUser.shared.RequestForming.features;

public class GetAllProducts
{
    public class GetAllProductsQuery : IRequest<ICollection<ProductDTO>>
    {
        public RequestParameters Parameters { get; set; }
    }

    public class Handler : IRequestHandler<GetAllProductsQuery, ICollection<ProductDTO>>
    {
        private readonly DB_Context dbcontext;
        private readonly IMapper mapper;

        public Handler(DB_Context context, IMapper mapper)
        {
            dbcontext = context;
            this.mapper = mapper;
        }

        public async Task<ICollection<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var data = await dbcontext.Products.ToListAsync(cancellationToken);
            var results = mapper.Map<ICollection<ProductDTO>>(data);
            return PagedList<ProductDTO>.PaginateList(results, results.Count, request.Parameters.PageNum, request.Parameters.PageSize);
        }
    }
}
