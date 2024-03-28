using AutoMapper;
using MediatR;
using E = WebUser.Domain.entities;
using WebUser.shared.RepoWrapper;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;

namespace WebUser.features.Category.Functions
{
    public class CreateCategory
    {
        //input
        public class CreateCategoryCommand : IRequest<CategoryDTO>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public ICollection<E.AttributeName> Attributes { get; set; }
            public ICollection<E.Category> Subcategories { get; set; }
            public E.Category ParentCategory { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<CreateCategoryCommand, CategoryDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<CategoryDTO> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = new E.Category
                {
                    Name = request.Name,
                    Attributes = request.Attributes,
                    Subcategories = request.Subcategories,
                    ParentCategory = request.ParentCategory,
                    ID=request.Id,
                    ParentCategoryId=request.ParentCategory.ID,
                };
                _repoWrapper.Category.Create(category);
                await _repoWrapper.SaveAsync();
                var results = _mapper.Map<CategoryDTO>(category);
                return results;
            }
        }

    }
}
