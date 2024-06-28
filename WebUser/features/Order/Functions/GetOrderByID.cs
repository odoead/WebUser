using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.Exceptions;
using WebUser.features.Order.DTO;

namespace WebUser.features.Order.Functions
{
    public class GetOrderByID
    {
        //input
        public class GetByIDQuery : IRequest<OrderDTO>
        {
            public int Id { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetByIDQuery, OrderDTO>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<OrderDTO> Handle(GetByIDQuery request, CancellationToken cancellationToken)
            {
                var order =
                    await dbcontext.Categories.FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken)
                    ?? throw new CategoryNotFoundException(request.Id);
                var results = mapper.Map<OrderDTO>(order);
                return results;
            }
        }
    }
}
