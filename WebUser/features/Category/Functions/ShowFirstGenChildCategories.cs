using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;

namespace WebUser.features.Category.Functions
{
    public class ShowFirstGenChildCategories
    {
        //input
        public class GetFirstGenChildCategoriesQuery : IRequest<ICollection<CategoryDTO>>
        {
            public int Id { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetFirstGenChildCategoriesQuery, ICollection<CategoryDTO>>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<CategoryDTO>> Handle(GetFirstGenChildCategoriesQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Categories.AnyAsync(q => q.ID == request.Id, cancellationToken: cancellationToken))
                    throw new CategoryNotFoundException(request.Id);
                var children = dbcontext.Categories.Where(q => q.ParentCategory.ID == request.Id) ?? throw new CategoryNotFoundException(-1);
                var results = mapper.Map<ICollection<CategoryDTO>>(children);
                return results;
            }
        }
    }
}
