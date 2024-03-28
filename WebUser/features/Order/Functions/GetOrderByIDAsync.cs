using AutoMapper;
using MediatR;
using E=WebUser.Domain.entities;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using WebUser.features.Order.DTO;

namespace WebUser.features.Order.Functions
{
    public class GetOrderByIDAsync
    {
        //input
        public class GetByIDQuery : IRequest<OrderDTO>
        {
            public int Id { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetByIDQuery, OrderDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<OrderDTO> Handle(GetByIDQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Order.IsExistsAsync(new ObjectID<E.Order>(request.Id)))
                {
                    var order = await _repoWrapper.Order.GetByIdAsync(new ObjectID<E.Order>(request.Id));
                    var results = _mapper.Map<OrderDTO>(order);
                    return results;
                }
                throw new CategoryNotFoundException(request.Id);
            }
        }
    }
}
