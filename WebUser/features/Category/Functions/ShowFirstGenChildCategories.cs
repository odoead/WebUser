using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
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

            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<ICollection<CategoryDTO>> Handle(GetFirstGenChildCategoriesQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Categories.AnyAsync(q => q.ID == request.Id, cancellationToken: cancellationToken))
                {
                    throw new CategoryNotFoundException(request.Id);
                }

                var children = dbcontext.Categories.Where(q => q.ParentCategoryID == request.Id);
                var results = new List<CategoryDTO>();
                foreach (var category in children)
                {
                    var categoryDTO = new CategoryDTO
                    {
                        ID = category.ID,
                        Name = category.Name,
                        Attributes = category
                            .Attributes?.Select(attr => new AttributeNameMinDTO { ID = attr.AttributeNameID, Name = attr.AttributeName.Name })
                            .ToList(),
                        ParentCategory =
                            category.ParentCategory != null
                                ? new CategoryMinDTO { ID = category.ParentCategory.ID, Name = category.ParentCategory.Name }
                                : null,
                        Subcategories = category.Subcategories?.Select(sub => new CategoryMinDTO { ID = sub.ID, Name = sub.Name }).ToList(),
                    };
                    results.Add(categoryDTO);
                }
                return results;
            }
        }
    }
}
