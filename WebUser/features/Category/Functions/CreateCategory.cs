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
            public int? ParentCategoryID { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreateCategoryCommand, CategoryDTO>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
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
                E.Category? parent = null;
                if (request.ParentCategoryID != null)
                {
                    parent = await dbcontext
                        .Categories.Where(q => q.ID == request.ParentCategoryID)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                }
                var category = new E.Category
                {
                    Name = request.Name,
                    Subcategories = subcategories,
                    ParentCategory = parent,
                };
                if (
                    !await dbcontext
                        .Categories.Include(q => q.Subcategories)
                        .Include(q => q.ParentCategory)
                        .AnyAsync(
                            q => q.Name == category.Name && q.Subcategories == category.Subcategories && q.ParentCategory == category.ParentCategory,
                            cancellationToken: cancellationToken
                        )
                )
                {
                    await dbcontext.Categories.AddAsync(category, cancellationToken);
                    parent?.Subcategories.Add(category);
                    await dbcontext.SaveChangesAsync(cancellationToken);

                    return new CategoryDTO
                    {
                        ID = category.ID,
                        Name = category.Name,
                        ParentCategory = parent != null ? new CategoryMinDTO { ID = parent.ID, Name = parent.Name } : null,
                        Subcategories = subcategories.Select(subcat => new CategoryMinDTO { ID = subcat.ID, Name = subcat.Name }).ToList(),
                        Attributes = null,
                    };
                }
                var alreadyExists = await dbcontext.Categories.FirstOrDefaultAsync(
                    q => q.Name == category.Name && q.Subcategories == category.Subcategories && q.ParentCategory == category.ParentCategory,
                    cancellationToken: cancellationToken
                );
                var results = new CategoryDTO
                {
                    ID = alreadyExists.ID,
                    Name = alreadyExists.Name,
                    ParentCategory = parent != null ? new CategoryMinDTO { ID = parent.ID, Name = parent.Name } : null,
                    Subcategories = subcategories.Select(subcat => new CategoryMinDTO { ID = subcat.ID, Name = subcat.Name }).ToList(),
                    Attributes = null,
                };

                return results;
            }
        }
    }
}
