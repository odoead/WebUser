using AutoMapper;
using MediatR;
using E=WebUser.Domain.entities;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Category.Functions
{
    public class GetCategoryByIDAsync
    {
        //input
        public class GetCategoryByIDQuery : IRequest<CategoryDTO>
        {
            public int Id { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetCategoryByIDQuery, CategoryDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<CategoryDTO> Handle(GetCategoryByIDQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Category.IsExistsAsync(new ObjectID<E.Category>(request.Id)))
                {
                    var categories = await _repoWrapper.Category.GetByIdAsync(new ObjectID<E.Category>(request.Id));
                    var results = _mapper.Map<CategoryDTO>(categories);
                    return results;
                }
                throw new CategoryNotFoundException(request.Id);
            }
        }
    }
}
