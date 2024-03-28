using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.discount.DTO;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.discount.Functions
{
    public class GetAllOrdersAsync
    {
        //input
        public class GetAllAsyncQuery : IRequest<ICollection<DiscountDTO>> { }
        //handler
        public class Handler : IRequestHandler<GetAllAsyncQuery, ICollection<DiscountDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<DiscountDTO>> Handle(GetAllAsyncQuery request, CancellationToken cancellationToken)
            {
                var discounts = await _repoWrapper.Discount.GetAllAsync();
                var results = _mapper.Map<ICollection<DiscountDTO>>(discounts);
                return results;
            }
        }

    }
}
