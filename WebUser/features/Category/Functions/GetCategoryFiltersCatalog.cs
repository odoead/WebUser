namespace WebUser.features.Category.Functions;

using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.shared.RepoWrapper;

public class GetCategoryFiltersCatalog
{
    //input
    public class GetCategoryFiltersCatalogQuery : IRequest<CategoryAttributeFiltersDTO>
    {
        public int Id { get; set; }
        public bool IncludeChildCategories { get; set; } //show filters only for only one category or include related subcategories
    }

    //handler
    public class Handler : IRequestHandler<GetCategoryFiltersCatalogQuery, CategoryAttributeFiltersDTO>
    {
        private readonly DB_Context dbcontext;
        private readonly IServiceWrapper service;

        public Handler(DB_Context context, IServiceWrapper service)
        {
            dbcontext = context;

            this.service = service;
        }

        public async Task<CategoryAttributeFiltersDTO> Handle(GetCategoryFiltersCatalogQuery request, CancellationToken cancellationToken)
        {
            var category =
                await dbcontext
                    .Categories.Include(c => c.Subcategories)
                    .Include(c => c.Attributes)
                    .ThenInclude(a => a.AttributeName)
                    .ThenInclude(a => a.AttributeValues)
                    .FirstOrDefaultAsync(c => c.ID == request.Id, cancellationToken) ?? throw new CategoryNotFoundException(request.Id);

            var subcategories = category.Subcategories;
            var parentRoute = await service.Category.GetParentCategoriesLine(request.Id);

            var attributes = request.IncludeChildCategories
                ? await GetAttributesForChildCategories(request.Id, cancellationToken)
                : category.Attributes.Select(a => a.AttributeName).ToList();

            var attributeNameValues = attributes
                .Select(attribute => new AttributeNameValueDTO
                {
                    AttributeName = new AttributeNameMinDTO { Name = attribute.Name, ID = attribute.ID },
                    Attributes = attribute.AttributeValues.Select(av => new AttributeValueDTO { ID = av.ID, Value = av.Value }).ToList(),
                })
                .ToList();

            return new CategoryAttributeFiltersDTO
            {
                Attributes = attributeNameValues,
                ID = category.ID,
                ParentRouteCategories = parentRoute.Select(p => new CategoryMinDTO { ID = p.ID, Name = p.Name }).ToList(),
                Subcategories = subcategories.Select(s => new CategoryMinDTO { ID = s.ID, Name = s.Name }).ToList(),
                IncludesChildCategories = request.IncludeChildCategories,
                Name = category.Name,
            };
        }

        private async Task<List<AttributeName>> GetAttributesForChildCategories(int categoryId, CancellationToken cancellationToken)
        {
            var childCategories = await service.Category.GetAllGenChildCategories(categoryId);
            var selectedCategoryIds = childCategories.Select(c => c.ID).Append(categoryId).ToList();

            return await dbcontext
                .AttributeNames.Include(a => a.AttributeValues)
                .Where(a => selectedCategoryIds.Contains(a.ID))
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}
