using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.OrderProduct.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.OrderProduct.Functions
{
    public class GetAllOrderProds
    {
        //input
        public class GetAllOrderProdsAsyncQuery : IRequest<ICollection<OrderProductDTO>>
        {
            public RequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllOrderProdsAsyncQuery, ICollection<OrderProductDTO>>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<OrderProductDTO>> Handle(GetAllOrderProdsAsyncQuery request, CancellationToken cancellationToken)
            {
                var OrderProducts = await dbcontext.OrderProducts.ToListAsync(cancellationToken: cancellationToken);
                var results = mapper.Map<ICollection<OrderProductDTO>>(OrderProducts);
                return PagedList<OrderProductDTO>.PaginateList(results, results.Count, request.Parameters.PageNum, request.Parameters.PageSize);
            }
        }
    }
}
