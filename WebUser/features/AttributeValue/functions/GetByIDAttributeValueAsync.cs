using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.AttributeValue.functions
{
    public class GetByIDAttributeValueAsync
    {
        //input
        public class GetByIDAttrValueQuery : IRequest<AttributeValueDTO>
        {
            public int Id { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetByIDAttrValueQuery, AttributeValueDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<AttributeValueDTO> Handle(GetByIDAttrValueQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.AttributeValue.IsExistsAsync(new ObjectID<E.AttributeValue>(request.Id)))
                {
                    var name = await _repoWrapper.AttributeValue.GetByIdAsync(new ObjectID<E.AttributeValue>(request.Id));
                    var results = _mapper.Map<AttributeValueDTO>(name);
                    return results;
                }
                else
                    throw new CategoryNotFoundException(request.Id);
            }
        }
    }
}
