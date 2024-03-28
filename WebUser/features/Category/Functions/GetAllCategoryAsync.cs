using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Category.Functions
{
    public class GetAllCategoryAsync
    {
        //input
        public class GetAllCategoryAsyncQuery : IRequest<ICollection<CategoryDTO>> { }
        //handler
        public class Handler : IRequestHandler<GetAllCategoryAsyncQuery, ICollection<CategoryDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<CategoryDTO>> Handle(GetAllCategoryAsyncQuery request, CancellationToken cancellationToken)
            {
                var categories = await _repoWrapper.Category.GetAllAsync();
                var results = _mapper.Map<ICollection<CategoryDTO>>(categories);
                return results;
            }
        }

    }
}
