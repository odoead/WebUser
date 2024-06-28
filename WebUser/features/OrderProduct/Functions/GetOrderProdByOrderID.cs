using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.OrderProduct.DTO;

namespace WebUser.features.OrderProduct.Functions
{
    public class GetOrderProdByOrderID
    {
        //input
        public class GetByOrderProdIDQuery : IRequest<ICollection<OrderProductDTO>>
        {
            public int OrderId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetByOrderProdIDQuery, ICollection<OrderProductDTO>>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<OrderProductDTO>> Handle(GetByOrderProdIDQuery request, CancellationToken cancellationToken)
            {
                var items = await dbcontext.OrderProducts.Where(q => q.Order.ID == request.OrderId).ToListAsync(cancellationToken: cancellationToken);
                return mapper.Map<ICollection<OrderProductDTO>>(items);
            }
        }
    }
}
