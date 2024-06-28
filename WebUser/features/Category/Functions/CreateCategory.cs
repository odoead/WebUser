using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Category.Functions
{
    public class CreateCategory
    {
        //input
        public class CreateCategoryCommand : IRequest<CategoryDTO>
        {
            public string Name { get; set; }
            public List<int>? SubcategoriesID { get; set; }
            public int ParentCategoryID { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreateCategoryCommand, CategoryDTO>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<CategoryDTO> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                List<E.Category> subcategories = new List<E.Category>();
                if (request.SubcategoriesID != null)
                {
                    subcategories = await dbcontext
                        .Categories.Where(q => request.SubcategoriesID.Contains(q.ID))
                        .ToListAsync(cancellationToken: cancellationToken);
                }
                var parent = await dbcontext
                    .Categories.Where(q => q.ID == request.ParentCategoryID)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                var category = new E.Category
                {
                    Name = request.Name,
                    Subcategories = subcategories,
                    ParentCategory = parent,
                };
                if (
                    !await dbcontext.Categories.AnyAsync(
                        q => q.Name == category.Name && q.Subcategories == category.Subcategories && q.ParentCategory == category.ParentCategory,
                        cancellationToken: cancellationToken
                    )
                )
                {
                    await dbcontext.Categories.AddAsync(category, cancellationToken);
                    //parent.Subcategories.Add(category);
                    //subcategories.Select(q => q.ParentCategory);
                    await dbcontext.SaveChangesAsync(cancellationToken);
                }
                var results = mapper.Map<CategoryDTO>(category);
                return results;
            }
        }
    }
}
