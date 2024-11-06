using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Coupon.DTO;
using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;
using WebUser.features.Product.DTO;
using WebUser.features.Promotion_TODO.DTO;
using WebUser.shared.extentions;
using E = WebUser.Domain.entities;

namespace WebUser.features.Product.Functions
{
    public class CreateProduct
    {
        //input
        public class CreateProductCommand : IRequest<ProductDTO>
        {
            public string Description { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public int Stock { get; set; }
            public ICollection<int> AttributeValuesID { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreateProductCommand, ProductDTO>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<ProductDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                var attributeValues = await dbcontext
                    .AttributeValues.Where(q => request.AttributeValuesID.Contains(q.ID))
                    .ToListAsync(cancellationToken: cancellationToken);
                var product = new E.Product
                {
                    Description = request.Description,
                    Name = request.Name,
                    Price = request.Price,
                    Stock = request.Stock,
                    DateCreated = DateTime.UtcNow,
                    ReservedStock = 0,
                };

                await ManyToManyEntitiesUpdater.UpateManyToManyRelationsAsync<E.Product, E.AttributeValue, E.ProductAttributeValue>(
                    dbcontext, product, product.AttributeValues, request.AttributeValuesID,
                    (prod, attrValue) =>
                    new E.ProductAttributeValue
                    {
                        Product = prod,
                        ProductID = prod.ID,
                        AttributeValue = attrValue,
                        AttributeValueID = attrValue.ID,
                    },
               async ids => await dbcontext.AttributeValues.Where(av => ids.Contains(av.ID)).ToListAsync(cancellationToken));

                await dbcontext.Products.AddAsync(product, cancellationToken);
                await dbcontext.SaveChangesAsync(cancellationToken);

                var productDTO = new ProductDTO
                {
                    ID = product.ID,
                    Description = product.Description,
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock,
                    ReservedStock = product.ReservedStock,
                    IsPurchasable = product.Stock > product.ReservedStock,
                    DateCreated = product.DateCreated,

                    AttributeValues = product
                        .AttributeValues.GroupBy(av => av.AttributeValue.AttributeName)
                        .Select(g => new AttributeNameValueDTO
                        {
                            AttributeName = new AttributeNameMinDTO { ID = g.Key.ID, Name = g.Key.Name },
                            Attributes = g.Select(av => new AttributeValueDTO { ID = av.AttributeValue.ID, Value = av.AttributeValue.Value })
                                .ToList(),
                        })
                        .ToList(),

                    Images = new List<ImageDTO>(),
                    Discounts = new List<DiscountMinDTO>(),
                    Coupons = new List<CouponMinDTO>(),
                    Promotions = new List<PromotionMinDTO>(),
                };

                return productDTO;
            }
        }
    }
}
