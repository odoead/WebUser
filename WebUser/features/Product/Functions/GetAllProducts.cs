namespace WebUser.features.Product.Functions;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Coupon.DTO;
using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;
using WebUser.features.Product.DTO;
using WebUser.features.Product.extensions;
using WebUser.features.Promotion_TODO.DTO;
using WebUser.shared.RequestForming.features;

public class GetAllProducts
{
    public class GetAllProductsQuery : IRequest<PagedList<ProductDTO>>
    {
        public ProductRequestParameters Parameters { get; set; }
    }

    public class Handler : IRequestHandler<GetAllProductsQuery, PagedList<ProductDTO>>
    {
        private readonly DB_Context dbcontext;

        public Handler(DB_Context context)
        {
            dbcontext = context;
        }

        public async Task<PagedList<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var data = dbcontext
                .Products.Include(p => p.Images)
                .Include(p => p.AttributeValues)
                .ThenInclude(av => av.AttributeValue)
                .ThenInclude(av => av.AttributeName)
                .Include(p => p.Discounts)
                .Include(p => p.Coupons)
                .Include(p => p.Promotions)
                .ThenInclude(q => q.Promotion)
                .AsQueryable();

            var srcProducts = await data.Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                .Take(request.Parameters.PageSize)
                .ToListAsync(cancellationToken);

            // Manual mapping from Products to ProductDTO
            var dtoProducts = srcProducts
                .Select(product => new ProductDTO
                {
                    ID = product.ID,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    ReservedStock = product.ReservedStock,
                    IsPurchasable = product.Stock > 0 && product.Stock > product.ReservedStock,
                    DateCreated = product.DateCreated,
                    Images = product.Images.Select(image => new ImageDTO { ID = image.ID, ImageContent = image.ImageContent }).ToList(),
                    AttributeValues = product
                        .AttributeValues.GroupBy(av => av.AttributeValue.AttributeName)
                        .Select(group => new AttributeNameValueDTO
                        {
                            AttributeName = new AttributeNameMinDTO { ID = group.Key.ID, Name = group.Key.Name },
                            Attributes = group
                                .Select(av => new AttributeValueDTO { ID = av.AttributeValue.ID, Value = av.AttributeValue.Value })
                                .ToList(),
                        })
                        .ToList(),
                    Discounts = product
                        .Discounts.Select(discount => new DiscountMinDTO
                        {
                            ID = discount.ID,
                            DiscountPercent = discount.DiscountPercent,
                            DiscountVal = discount.DiscountVal,
                            IsActive = Discount.IsActive(discount),
                        })
                        .ToList(),
                    Coupons = product
                        .Coupons.Select(coupon => new CouponMinDTO
                        {
                            ID = coupon.ID,
                            IsActive = !coupon.IsActivated,
                            DiscountVal = coupon.DiscountVal,
                            DiscountPercent = coupon.DiscountPercent,
                        })
                        .ToList(),
                    Promotions = product
                        .Promotions.Select(promotion => new PromotionMinDTO { ID = promotion.Promotion.ID, Name = promotion.Promotion.Name })
                        .ToList(),
                })
                .ToList();

            var pagedList = PagedList<ProductDTO>.PaginateList(
                source: dtoProducts,
                totalCount: await data.CountAsync(cancellationToken: cancellationToken),
                pageNumber: request.Parameters.PageNumber,
                pageSize: request.Parameters.PageSize
            );

            return pagedList;
        }
    }
}
