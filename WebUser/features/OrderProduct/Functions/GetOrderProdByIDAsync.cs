using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.features.OrderProduct.DTO;
using WebUser.features.OrderProduct.Exceptions;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.OrderProduct.Functions
{
    public class GetOrderProductByIDAsync
    {
        //input
        public class GetByOrderProdIDQuery : IRequest<OrderProductDTO>
        {
            public int Id { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetByOrderProdIDQuery, OrderProductDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<OrderProductDTO> Handle(GetByOrderProdIDQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Category.IsExistsAsync(request.Id))
                {
                    var categories = await _repoWrapper.OrderProduct.GetByIdAsync(request.Id);
                    var results = _mapper.Map<OrderProductDTO>(categories);
                    return results;
                }
                throw new OrderProductNotFoundException(request.Id);
            }
        }
    }
}
