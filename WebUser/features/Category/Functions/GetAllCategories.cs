using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.Category.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.Category.Functions
{
    public class GetAllCategories
    {
        //input
        public class GetAllCategoryAsyncQuery : IRequest<PagedList<CategoryDTO>>
        {
            public CategoryRequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllCategoryAsyncQuery, PagedList<CategoryDTO>>
        {
            private readonly DB_Context dbcontext;


            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<PagedList<CategoryDTO>> Handle(GetAllCategoryAsyncQuery request, CancellationToken cancellationToken)
            {
                var data = dbcontext
                    .Categories.Include(q => q.Attributes)
                    .ThenInclude(q => q.AttributeName)
                    .Include(q => q.ParentCategory)
                    .Include(q => q.Subcategories)
                    .AsQueryable();

                var src = await data.Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                    .Take(request.Parameters.PageSize)
                    .ToListAsync(cancellationToken);
                var dto = new List<CategoryDTO>();

                foreach (var category in src)
                {
                    var categoryDTO = new CategoryDTO
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

                    dto.Add(categoryDTO);
                }

                var pagedList = PagedList<CategoryDTO>.PaginateList(
                    source: dto,
                    totalCount: await data.CountAsync(cancellationToken: cancellationToken),
                    pageNumber: request.Parameters.PageNumber,
                    pageSize: request.Parameters.PageSize
                );

                return pagedList;
            }
        }
    }
}
