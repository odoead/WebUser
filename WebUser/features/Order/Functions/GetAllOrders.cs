using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.Order.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.Order.Functions
{
    public class GetAllOrders
    {
        //input
        public class GetAllOrdersAsyncQuery : IRequest<ICollection<OrderDTO>>
        {
            public RequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllOrdersAsyncQuery, ICollection<OrderDTO>>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<OrderDTO>> Handle(GetAllOrdersAsyncQuery request, CancellationToken cancellationToken)
            {
                var order = await dbcontext.Orders.ToListAsync(cancellationToken: cancellationToken);
                var results = mapper.Map<ICollection<OrderDTO>>(order);
                return PagedList<OrderDTO>.PaginateList(results, results.Count, request.Parameters.PageNum, request.Parameters.PageSize);
            }
        }
    }
}
