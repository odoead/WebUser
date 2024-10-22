namespace WebUser.features.Category.Functions;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.shared.RepoWrapper;

public class GetSearchFiltesCatalog
{
    //input
    public class GetSearchFiltesCatalogQuery : IRequest<SearchAttributeFiltersDTO>
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
    }

    //handler
    public class Handler : IRequestHandler<GetSearchFiltesCatalogQuery, SearchAttributeFiltersDTO>
    {
        private readonly DB_Context dbcontext;
        private readonly IServiceWrapper service;

        public Handler(DB_Context context, IServiceWrapper service)
        {
            dbcontext = context;
            this.service = service;
        }

        public async Task<SearchAttributeFiltersDTO> Handle(GetSearchFiltesCatalogQuery request, CancellationToken cancellationToken)
        {
            var products = await dbcontext
                .Products.Include(p => p.AttributeValues)
                .ThenInclude(pav => pav.AttributeValue)
                .ThenInclude(av => av.AttributeName)
                .ThenInclude(an => an.Categories)
                .ThenInclude(anc => anc.Category)
                .ToListAsync(cancellationToken: cancellationToken);

            var attributeNames = products.SelectMany(p => p.AttributeValues.Select(av => av.AttributeValue.AttributeName)).Distinct().ToList();

            var attributeNameValues = attributeNames
                .Select(attribute => new AttributeNameValueDTO
                {
                    AttributeName = new AttributeNameMinDTO { Name = attribute.Name, ID = attribute.ID },
                    Attributes = attribute.AttributeValues.Select(av => new AttributeValueDTO { ID = av.ID, Value = av.Value }).ToList(),
                })
                .ToList();

            var categories = products
                .SelectMany(p => p.AttributeValues.SelectMany(av => av.AttributeValue.AttributeName.Categories))
                .GroupBy(c => c.CategoryID)
                .Select(g => g.FirstOrDefault())
                .ToList();
            var categoryDTOs = categories.Select(c => new CategoryMinDTO { Name = c.Category.Name, ID = c.CategoryID }).ToList();

            return new SearchAttributeFiltersDTO { Attributes = attributeNameValues, СategoriesOfFoundItems = categoryDTOs };
        }
    }
}
