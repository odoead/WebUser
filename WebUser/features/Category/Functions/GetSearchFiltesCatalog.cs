namespace WebUser.features.Category.Functions;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Product.extensions;

public class GetSearchFiltesCatalog
{
    //input
    public class GetSearchFiltesCatalogQuery : IRequest<SearchAttributeFiltersDTO>
    {
        public string RequestName { get; set; }
    }

    //handler
    public class Handler : IRequestHandler<GetSearchFiltesCatalogQuery, SearchAttributeFiltersDTO>
    {
        private readonly DB_Context dbcontext;

        public Handler(DB_Context context)
        {
            dbcontext = context;
        }

        public async Task<SearchAttributeFiltersDTO> Handle(GetSearchFiltesCatalogQuery request, CancellationToken cancellationToken)
        {
            //select all products with suitable names (no paging)
            var rr = dbcontext
                .Products.Include(p => p.AttributeValues)
                .ThenInclude(pav => pav.AttributeValue)
                .ThenInclude(av => av.AttributeName)
                .ThenInclude(an => an.Categories)
                .ThenInclude(anc => anc.Category)
                .SearchByName(request.RequestName).AsQueryable();
            var products = await rr
            .ToListAsync(cancellationToken: cancellationToken);

            //get all unique attr names 
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
