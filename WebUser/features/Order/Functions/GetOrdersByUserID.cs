using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.shared.RepoWrapper;
using WebUser.shared;
using WebUser.features.Order.DTO;
using E=WebUser.Domain.entities;
using WebUser.features.Order.Exceptions;

namespace WebUser.features.Order.Functions
{
    public class GetOrdersByUser
    {

        //input
        public class GetOrdersByUserQuery : IRequest<ICollection<OrderDTO>>
        {
            public int UserId { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetOrdersByUserQuery, ICollection<OrderDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<OrderDTO>> Handle(GetOrdersByUserQuery query, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.user.IsExistsAsync(new ObjectID<E.Order>(query.UserId)))
                {
                    var order = await _repoWrapper.Order.GetByUserIdAsync(new ObjectID<E.User>(query.UserId));
                    if(order== null)
                    {
                        throw new OrderNotFoundException(-1);
                    }
                    var results = _mapper.Map<ICollection<OrderDTO>>(order);
                    return results;
                }
                throw new CategoryNotFoundException(-1);
            }
        }
    }
}
