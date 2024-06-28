using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.Cart.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.Cart.functions
{
    public class GetAllCarts
    {
        //input
        public class GetAllCartsQuery : IRequest<ICollection<CartDTO>>
        {
            public RequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllCartsQuery, ICollection<CartDTO>>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<CartDTO>> Handle(GetAllCartsQuery request, CancellationToken cancellationToken)
            {
                var carts = await dbcontext.Carts.ToListAsync(cancellationToken);
                var results = mapper.Map<ICollection<CartDTO>>(carts);
                return PagedList<CartDTO>.PaginateList(results, results.Count, request.Parameters.PageNum, request.Parameters.PageSize);
            }
        }
    }
}
