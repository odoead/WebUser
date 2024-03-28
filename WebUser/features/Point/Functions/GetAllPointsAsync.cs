using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Point.DTO;
using WebUser.features.Promotion.DTO;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Point.Functions
{
    public class GetAllPromotionsAsync
    {
        //input
        public class GetAllOrderProdsAsyncQuery : IRequest<ICollection<PointDTO>> { }
        //handler
        public class Handler : IRequestHandler<GetAllOrderProdsAsyncQuery, ICollection<PointDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<PointDTO>> Handle(GetAllOrderProdsAsyncQuery request, CancellationToken cancellationToken)
            {
                var categories = await _repoWrapper.Point.GetAllAsync();
                var results = _mapper.Map<ICollection<PointDTO>>(categories);
                return results;
            }
        }

    }
}
