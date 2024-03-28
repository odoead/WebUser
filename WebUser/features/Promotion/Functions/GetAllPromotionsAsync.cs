using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Promotion.DTO;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Promotion.Functions
{
    public class GetAllPromotionsAsync
    {
        //input
        public class GetAllOrderProdsAsyncQuery : IRequest<ICollection<PromotionDTO>> { }
        //handler
        public class Handler : IRequestHandler<GetAllOrderProdsAsyncQuery, ICollection<PromotionDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<PromotionDTO>> Handle(GetAllOrderProdsAsyncQuery request, CancellationToken cancellationToken)
            {
                var categories = await _repoWrapper.Promotion.GetAllAsync();
                var results = _mapper.Map<ICollection<PromotionDTO>>(categories);
                return results;
            }
        }

    }
}
