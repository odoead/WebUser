using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.OrderProduct.DTO;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.OrderProduct.Functions
{
    public class GetAllPointsAsync
    {
        //input
        public class GetAllOrderProdsAsyncQuery : IRequest<ICollection<OrderProductDTO>> { }
        //handler
        public class Handler : IRequestHandler<GetAllOrderProdsAsyncQuery, ICollection<OrderProductDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<OrderProductDTO>> Handle(GetAllOrderProdsAsyncQuery request, CancellationToken cancellationToken)
            {
                var categories = await _repoWrapper.OrderProduct.GetAllAsync();
                var results = _mapper.Map<ICollection<OrderProductDTO>>(categories);
                return results;
            }
        }

    }
}
