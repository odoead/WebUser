using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.AttributeValue.functions
{
    public class GetAllAttrValueAsync
    {
        //input
        public class GetAllAttrValueQuery : IRequest<ICollection<AttributeValueDTO>> { }
        //handler
        public class Handler : IRequestHandler<GetAllAttrValueQuery, ICollection<AttributeValueDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper repoWrapper, IMapper mapper)
            {
                _repoWrapper = repoWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<AttributeValueDTO>> Handle(GetAllAttrValueQuery request, CancellationToken cancellationToken)
            {
                var values = await _repoWrapper.AttributeValue.GetAllAsync();
                var results = _mapper.Map<ICollection<AttributeValueDTO>>(values);
                return results;
            }
        }

    }
}
