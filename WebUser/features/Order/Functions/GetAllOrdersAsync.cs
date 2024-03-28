using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Order.DTO;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Order.Functions
{
    public class GetAllOrdersAsync
    {
        //input
        public class GetAllAsyncQuery : IRequest<ICollection<OrderDTO>> { }
        //handler
        public class Handler : IRequestHandler<GetAllAsyncQuery, ICollection<OrderDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<OrderDTO>> Handle(GetAllAsyncQuery request, CancellationToken cancellationToken)
            {
                var categories = await _repoWrapper.Order.GetAllAsync();
                var results = _mapper.Map<ICollection<OrderDTO>>(categories);
                return results;
            }
        }

    }
}
