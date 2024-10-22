using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;

namespace WebUser.features.Category.Functions
{
    public class GetCategoryByID
    {
        //input
        public class GetCategoryByIDQuery : IRequest<CategoryDTO>
        {
            public int Id { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetCategoryByIDQuery, CategoryDTO>
        {

            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<CategoryDTO> Handle(GetCategoryByIDQuery request, CancellationToken cancellationToken)
            {
                //get category
                var category =
                    await dbcontext
                        .Categories.Include(q => q.Attributes)
                        .ThenInclude(q => q.AttributeName)
                        .Include(q => q.ParentCategory)
                        .Include(q => q.Subcategories)
                        .FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken: cancellationToken)
                    ?? throw new CategoryNotFoundException(request.Id);

                var results = new CategoryDTO
                {
                    ID = category.ID,
                    Name = category.Name,
                    Attributes = category
                        .Attributes.Select(attr => new AttributeNameMinDTO { ID = attr.AttributeName.ID, Name = attr.AttributeName.Name })
                        .ToList(),

                    ParentCategory =
                        category.ParentCategory != null
                            ? new CategoryMinDTO { ID = category.ParentCategory.ID, Name = category.ParentCategory.Name }
                            : null,
                    Subcategories = category.Subcategories.Select(sub => new CategoryMinDTO { ID = sub.ID, Name = sub.Name }).ToList(),
                };
                return results;
            }
        }
    }
}
