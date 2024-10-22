using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.discount.DTO;
using WebUser.features.Product.DTO;
using WebUser.features.Product.Exceptions;
using E = WebUser.Domain.entities;

namespace WebUser.features.Product.Functions
{
    public class AddDiscountToProduct
    {
        //input
        public class AddDiscountToProductCommand : IRequest<DiscountDTO>
        {
            public int ProductId { get; set; }
            public DateTime ActiveFrom { get; set; }
            public DateTime ActiveTo { get; set; }
            public double? DiscountValue { get; set; }
            public int? DiscountPercent { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<AddDiscountToProductCommand, DiscountDTO>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<DiscountDTO> Handle(AddDiscountToProductCommand request, CancellationToken cancellationToken)
            {
                var product =
                    await dbcontext.Products.FirstOrDefaultAsync(q => q.ID == request.ProductId, cancellationToken: cancellationToken)
                    ?? throw new ProductNotFoundException(request.ProductId);

                var discount = new E.Discount
                {
                    ActiveFrom = request.ActiveFrom,
                    ActiveTo = request.ActiveTo,
                    CreatedAt = DateTime.UtcNow,
                    Product = product,
                    DiscountVal = request.DiscountValue > 0 ? (double)request.DiscountValue : 0,
                    DiscountPercent = request.DiscountPercent > 0 ? request.DiscountPercent : 0,
                };

                await dbcontext.Discounts.AddAsync(discount, cancellationToken);
                await dbcontext.SaveChangesAsync(cancellationToken);

                var resultDTO = new DiscountDTO
                {
                    ID = discount.ID,
                    Product = new ProductMinDTO
                    {
                        ID = discount.Product.ID,
                        Name = discount.Product.Name,
                        Price = discount.Product.Price,
                    },
                    CreatedAt = discount.CreatedAt,
                    ActiveFrom = discount.ActiveFrom,
                    ActiveTo = discount.ActiveTo,
                    DiscountVal = discount.DiscountVal,
                    DiscountPercent = discount.DiscountPercent,
                    IsActive = E.Discount.IsActive(discount),
                };

                return resultDTO;
            }
        }
    }
}
