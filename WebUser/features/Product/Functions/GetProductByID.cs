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
using WebUser.features.Product.Exceptions;
using WebUser.features.Promotion_TODO.DTO;

public class GetProductByID
{
    //input
    public class GetProductByIDQuery : IRequest<ProductDTO>
    {
        public int Id { get; set; }
    }

    //handler
    public class Handler : IRequestHandler<GetProductByIDQuery, ProductDTO>
    {
        private readonly DB_Context dbcontext;

        public Handler(DB_Context context)
        {
            dbcontext = context;
        }

        public async Task<ProductDTO> Handle(GetProductByIDQuery request, CancellationToken cancellationToken)
        {
            var data =
                await dbcontext
                    .Products.Include(q => q.AttributeValues)
                    .ThenInclude(q => q.AttributeValue)
                    .ThenInclude(q => q.AttributeName)
                    .Include(q => q.Discounts)
                    .Include(q => q.Coupons).Include(q => q.Promotions).ThenInclude(q => q.Promotion)
                    .FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken: cancellationToken)
                ?? throw new ProductNotFoundException(request.Id);
            var res = new ProductDTO
            {
                ID = data.ID,
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                Stock = data.Stock,
                ReservedStock = data.ReservedStock,
                IsPurchasable = data.Stock > 0 && data.Stock > data.ReservedStock,
                DateCreated = data.DateCreated,
                Images = data.Images.Select(image => new ImageDTO { ID = image.ID, ImageContent = image.ImageContent }).ToList(),
                AttributeValues = data
                    .AttributeValues.GroupBy(av => av.AttributeValue.AttributeName)
                    .Select(group => new AttributeNameValueDTO
                    {
                        AttributeName = new AttributeNameMinDTO { ID = group.Key.ID, Name = group.Key.Name },
                        Attributes = group
                            .Select(av => new AttributeValueDTO { ID = av.AttributeValue.ID, Value = av.AttributeValue.Value })
                            .ToList(),
                    })
                    .ToList(),
                Discounts = data
                    .Discounts.Select(discount => new DiscountMinDTO
                    {
                        ID = discount.ID,
                        DiscountPercent = discount.DiscountPercent,
                        DiscountVal = discount.DiscountVal,
                        IsActive = Discount.IsActive(discount),
                    })
                    .ToList(),
                Coupons = data
                    .Coupons.Select(coupon => new CouponMinDTO
                    {
                        ID = coupon.ID,
                        IsActive = !coupon.IsActivated,
                        DiscountVal = coupon.DiscountVal,
                        DiscountPercent = coupon.DiscountPercent,
                    })
                    .ToList(),
                Promotions = data
                    .Promotions.Select(promotion => new PromotionMinDTO { ID = promotion.Promotion.ID, Name = promotion.Promotion.Name, })
                    .ToList(),
            };
            return res;
        }
    }
}
