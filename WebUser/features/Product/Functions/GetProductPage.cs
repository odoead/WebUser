namespace WebUser.features.Product.Functions;

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Product.DTO;
using WebUser.features.Product.Exceptions;

public class GetProductPage
{
    //input
    public class GetProductPageByIDRequest : IRequest<ProductPageDTO>
    {
        public int Id { get; set; }
    }

    //handler
    public class Handler : IRequestHandler<GetProductPageByIDRequest, ProductPageDTO>
    {
        private readonly IMapper mapper;
        private readonly DB_Context dbcontext;

        public Handler(IMapper mapper, DB_Context context)
        {
            this.mapper = mapper;
            dbcontext = context;
        }

        public async Task<ProductPageDTO> Handle(GetProductPageByIDRequest request, CancellationToken cancellationToken)
        {
            var data = await dbcontext.Products.FirstOrDefaultAsync(q => q.ID == request.Id) ?? throw new ProductNotFoundException(request.Id);
            var res = mapper.Map<ProductPageDTO>(data);
            return res;
        }
    }
}
