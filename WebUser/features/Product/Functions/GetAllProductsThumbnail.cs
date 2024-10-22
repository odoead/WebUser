namespace WebUser.features.Product.Functions;

using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;
using WebUser.features.Product.DTO;
using WebUser.features.Product.extensions;
using WebUser.shared.RepoWrapper;
using WebUser.shared.RequestForming.features;

public class GetAllProductsThumbnail
{
    //input
    public class GetAllProductsThumbnailQuery : IRequest<PagedList<ProductThumbnailDTO>>
    {
        public ProductRequestParameters Parameters { get; set; }
        public int? CategoryId { get; set; } = 0; //if category id is provided than searches in category/subcategories,

        //if not than search starts from the root category
        public bool IncludeChildCategories { get; set; } //show products only for only one category or include related subcategories
    }

    //handler
    public class Handler : IRequestHandler<GetAllProductsThumbnailQuery, PagedList<ProductThumbnailDTO>>
    {
        private readonly DB_Context dbcontext;
        private readonly IServiceWrapper service;

        public Handler(DB_Context context, IServiceWrapper service)
        {
            dbcontext = context;

            this.service = service;
        }

        public async Task<PagedList<ProductThumbnailDTO>> Handle(GetAllProductsThumbnailQuery query, CancellationToken cancellationToken)
        {
            // Initialize a list to store the category IDs to search
            List<int> categoryIds = new List<int>();

            // If CategoryId is null, get the child categories of the first category
            if (query.CategoryId == null)
            {
                var rootCategoryId = 1; // Replace with logic to get the actual root category if necessary
                var childCategories = await service.Category.GetAllGenChildCategories(rootCategoryId);
                categoryIds.Add(rootCategoryId);
                categoryIds.AddRange(childCategories.Select(c => c.ID));
            }
            else
            {
                // If CategoryId is provided and IncludeChildCategories is true, include the child categories as well
                if (query.IncludeChildCategories)
                {
                    var childCategories = await service.Category.GetAllGenChildCategories(query.CategoryId.Value);
                    categoryIds.Add(query.CategoryId.Value);
                    categoryIds.AddRange(childCategories.Select(c => c.ID)); // Add all child categories
                }
                else
                {
                    // Only add the specified CategoryId
                    categoryIds.Add(query.CategoryId.Value);
                }
            }

            // Query the products based on the resolved category IDs
            var productsQuery = dbcontext
                .Products.Include(p => p.AttributeValues)
                .ThenInclude(av => av.AttributeValue)
                .ThenInclude(av => av.AttributeName)
                .ThenInclude(an => an.Categories)
                .Where(p => p.AttributeValues.Any(av => av.AttributeValue.AttributeName.Categories.Any(c => categoryIds.Contains(c.CategoryID))))
                .SearchByName(query.Parameters.RequestName)
                .Filter(query.Parameters.AttributeValueIDs, query.Parameters.MinPrice, query.Parameters.MaxPrice);

            var srcProducts = await productsQuery
                .Skip((query.Parameters.PageNumber - 1) * query.Parameters.PageSize)
                .Take(query.Parameters.PageSize)
                .Sort(query.Parameters.OrderBy)
                .ToListAsync(cancellationToken);

            // Manual mapping from Products to ProductThumbnailDTO
            var dtoProducts = srcProducts
                .Select(product => new ProductThumbnailDTO
                {
                    ID = product.ID,
                    Name = product.Name,
                    BasePrice = product.Price,
                    IsPurchasable = product.Stock > 0 && product.Stock > product.ReservedStock,
                    Images = product.Images.Select(image => new ImageDTO { ID = image.ID, ImageContent = image.ImageContent }).ToList(),
                    Discounts = product
                        .Discounts.Select(discount => new DiscountMinDTO
                        {
                            ID = discount.ID,
                            DiscountVal = discount.DiscountVal,
                            DiscountPercent = discount.DiscountPercent,
                            IsActive = Discount.IsActive(discount),
                        })
                        .ToList(),
                })
                .ToList();

            var pagedList = PagedList<ProductThumbnailDTO>.PaginateList(
                source: dtoProducts,
                totalCount: await productsQuery.CountAsync(cancellationToken),
                pageNumber: query.Parameters.PageNumber,
                pageSize: query.Parameters.PageSize
            );

            return pagedList;
        }
        /*public async Task<PagedList<ProductThumbnailDTO>> Handle(GetAllProductsThumbnailQuery query, CancellationToken cancellationToken)
        {
            var queryCategoriesId=query.IncludeChildCategories ? query.CategoryId : await service.Category.GetAllGenChildCategories(query.CategoryId);
            var pagedList = PagedList<ProductThumbnailDTO>.PaginateList(
                source: mapper.Map<ICollection<ProductThumbnailDTO>>(
                    await dbcontext
                        .Products.Include(q => q.AttributeValues)
                        .ThenInclude(q => q.AttributeValue)
                        .ThenInclude(q => q.AttributeName)
                        .ThenInclude(q => q.Categories)
                        .Where(p =>
                            p.AttributeValues.Any(av => av.AttributeValue.AttributeName.Categories.Any(c => c.CategoryID == query.CategoryId))
                        )
                        .Skip((query.Parameters.PageNumber - 1) * query.Parameters.PageSize)
                        .Take(query.Parameters.PageSize)
                        .SearchByName(query.Parameters.RequestName)
                        .Filter(query.Parameters.AttributeValueIDs, query.Parameters.MinPrice, query.Parameters.MaxPrice)
                        .Sort(query.Parameters.OrderBy)
                        .ToListAsync(cancellationToken)
                ),
                totalCount: await dbcontext.AttributeNames.CountAsync(cancellationToken: cancellationToken),
                pageNumber: query.Parameters.PageNumber,
                pageSize: query.Parameters.PageSize
            );
            return pagedList;
        }*/
    }
}
