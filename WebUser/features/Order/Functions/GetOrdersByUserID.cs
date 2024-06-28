using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Order.DTO;
using WebUser.features.User.Exceptions;

namespace WebUser.features.Order.Functions
{
    public class GetOrdersByUser
    {
        //input
        public class GetOrdersByUserQuery : IRequest<ICollection<OrderDTO>>
        {
            public string UserId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetOrdersByUserQuery, ICollection<OrderDTO>>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<OrderDTO>> Handle(GetOrdersByUserQuery request, CancellationToken cancellationToken)
            {
                var user =
                    await dbcontext.Users.FirstOrDefaultAsync(q => q.Id == request.UserId, cancellationToken: cancellationToken)
                    ?? throw new UserNotFoundException(request.UserId);
                var orders = await dbcontext.Orders.Where(q => q.User.Id == user.Id).ToListAsync(cancellationToken: cancellationToken);
                return mapper.Map<ICollection<OrderDTO>>(orders);
            }
        }
    }
}
