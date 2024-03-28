using AutoMapper;
using MediatR;
using WebUser.features.Point.Exceptions;
using WebUser.features.Promotion.DTO;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Point.Functions
{
    public class GetPromotionByIDAsync
    {
        //input
        public class GetByOrderProdIDQuery : IRequest<PromotionDTO>
        {
            public int Id { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetByOrderProdIDQuery, PromotionDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<PromotionDTO> Handle(GetByOrderProdIDQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Category.IsExistsAsync(request.Id))
                {
                    var categories = await _repoWrapper.Point.GetByIdAsync(request.Id);
                    var results = _mapper.Map<PromotionDTO>(categories);
                    return results;
                }
                throw new PointNotFoundException(request.Id);
            }
        }
    }
}
