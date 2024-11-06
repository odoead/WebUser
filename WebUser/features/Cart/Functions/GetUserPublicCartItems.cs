namespace WebUser.features.Cart.functions;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.Cart.DTO;
using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;
using WebUser.PricingService.DTO;
using WebUser.shared.RepoWrapper;

public class GetUserPublicCartItems
{
    //input
    public class GetUserPublicCartItemsQuery : IRequest<PublicCartItemsDTO>
    {
        public string UserId { get; set; } = string.Empty;
    }

    //handler
    public class Handler : IRequestHandler<GetUserPublicCartItemsQuery, PublicCartItemsDTO>
    {
        private readonly DB_Context dbcontext;
        private readonly UserManager<User> userManager;
        private readonly IServiceWrapper service;

        public Handler(DB_Context context, UserManager<User> userManager, IServiceWrapper serviceWrapper)
        {
            dbcontext = context;
            this.userManager = userManager;
            this.service = serviceWrapper;
        }

        public async Task<PublicCartItemsDTO> Handle(GetUserPublicCartItemsQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.UserId);
            var cartItems = await dbcontext
                .CartItems.Include(q => q.Cart)
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Images)
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Discounts)
                .Where(ci => ci.Cart.UserID == request.UserId)
                .ToListAsync(cancellationToken);

            var orderProducts = cartItems.Select(ci => (ci.ProductID, ci.Amount)).ToList();

            var (orderResult, nopointnull, activatedPointsnull, actviatedCouponsnull) = await service.Pricing.GenerateOrderAsync(orderProducts, user, string.Empty, 0);
            var publicCartItemsDTO = new PublicCartItemsDTO
            {
                CartItems = new List<CartItemThumbnailDTO>(),
                TotalCost = orderResult.OrderCost,
            };
            foreach (var productCalculation in orderResult.Products)
            {
                var product = cartItems.First(ci => ci.ProductID == productCalculation.ProductId).Product;

                publicCartItemsDTO.CartItems.Add(
                    new CartItemThumbnailDTO
                    {
                        ID = cartItems.First(ci => ci.ProductID == productCalculation.ProductId).ID,
                        ProductId = product.ID,
                        Name = product.Name,
                        BasePrice = product.Price,
                        FinalSinglePrice = productCalculation.FinalSinglePrice,
                        Discount = productCalculation.AppliedDiscounts.Count > 0 ? new PublicDiscountDTO
                        {
                            DiscountValue = productCalculation.AppliedDiscounts.Where(d => d.ValueTypes.Contains(DiscountValueType.Absolute))
                            ?.Sum(d => (int?)d.AbsoluteDiscountValue) ?? 0,
                            Percent = productCalculation.AppliedDiscounts.FirstOrDefault(d => d.ValueTypes.Contains(DiscountValueType.Percentage))
                            ?.Percent ?? 0,
                            DiscountPercent = productCalculation.AppliedDiscounts.Where(d => d.ValueTypes.Contains(DiscountValueType.Percentage))
                            ?.Sum(d => (int?)d.PercentDiscountValue) ?? 0,
                        } : null,
                        IsPurchasable = Product.IsPurchasable(product, productCalculation.Quantity),
                        Images = product.Images.Select(img => new ImageDTO { ID = img.ID, ImageContent = img.ImageContent }).ToList(),
                        CartItemStock = productCalculation.Quantity,
                        TotalFreeStock = product.Stock - product.ReservedStock
                        ,
                    }
                );
            }
            return publicCartItemsDTO;
        }
    }
}
