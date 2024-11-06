using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.exceptions;
using WebUser.features.discount.DTO;
using WebUser.features.Product.DTO;
using WebUser.features.Product.Exceptions;
using E = WebUser.Domain.entities;

namespace WebUser.features.discount.Functions
{
    public class GetDiscountsByProductID
    {
        //input
        public class GetDiscountByProductIDQuery : IRequest<ICollection<DiscountDTO>>
        {
            public int ProductId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetDiscountByProductIDQuery, ICollection<DiscountDTO>>
        {

            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<ICollection<DiscountDTO>> Handle(GetDiscountByProductIDQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Products.AnyAsync(q => q.ID == request.ProductId, cancellationToken: cancellationToken))
                {
                    throw new ProductNotFoundException(request.ProductId);
                }

                var discounts =
                    await dbcontext
                        .Discounts.Include(q => q.Product)
                        .Where(q => q.ProductID == request.ProductId)
                        .ToListAsync(cancellationToken: cancellationToken) ?? throw new RelatedEntityNotFoundException(nameof(E.Discount), nameof(GetDiscountsByProductID), "Handle");

                var discountDTOs = new List<DiscountDTO>();
                foreach (var discount in discounts)
                {
                    var discountDto = new DiscountDTO
                    {
                        ID = discount.ID,
                        ActiveFrom = discount.ActiveFrom,
                        ActiveTo = discount.ActiveTo,
                        CreatedAt = discount.CreatedAt,
                        DiscountPercent = discount.DiscountPercent,
                        DiscountVal = discount.DiscountVal,
                        Product = new ProductMinDTO
                        {
                            ID = discount.Product.ID,
                            Name = discount.Product.Name,
                            Price = discount.Product.Price,
                        },
                        IsActive = E.Discount.IsActive(discount),
                    };

                    discountDTOs.Add(discountDto);
                }
                var results = discountDTOs;
                return results;
            }
        }
    }
}
