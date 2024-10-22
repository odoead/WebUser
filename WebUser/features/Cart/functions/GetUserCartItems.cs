namespace WebUser.features.Cart.functions;

using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.Cart.DTO;
using WebUser.features.CartItem.DTO;
using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;

public class GetUserCartItems
{
    //input
    public class GetUserCartItemsQuery : IRequest<PublicCartItemsDTO>
    {
        public ClaimsPrincipal UserClaims { get; set; }
    }

    //handler
    public class Handler : IRequestHandler<GetUserCartItemsQuery, PublicCartItemsDTO>
    {
        private readonly DB_Context dbcontext;
        private readonly UserManager<User> userManager;

        public Handler(DB_Context context, UserManager<User> userManager)
        {
            dbcontext = context;
            this.userManager = userManager;
        }

        public async Task<PublicCartItemsDTO> Handle(GetUserCartItemsQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);

            var cartItems = await dbcontext
                .CartItems.Include(q => q.Cart)
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Images)
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Discounts)
                .Where(ci => ci.Cart.UserID == userId)
                .ToListAsync(cancellationToken);

            var cartItemsDTO = cartItems
                .Select(ci => new CartItemThumbnailDTO
                {
                    ID = ci.ID,
                    ProductId = ci.Product.ID,
                    Name = ci.Product.Name,
                    BasePrice = ci.Product.Price,
                    CartItemStock = ci.Amount,
                    TotalFreeStock = ci.Product.Stock - ci.Product.ReservedStock,
                    IsPurchasable = (ci.Product.Stock - ci.Product.ReservedStock) > 0 && ci.Product.Stock > 0,
                    Images = ci.Product.Images.Take(1).Select(image => new ImageDTO { ID = image.ID, ImageContent = image.ImageContent }).ToList(),
                    Discounts = ci.Product.Discounts.Select(d => new DiscountMinDTO { DiscountPercent = d.DiscountPercent, DiscountVal = d.DiscountVal, ID = d.ID, IsActive = Discount.IsActive(d) })
                        .ToList(),
                })
                .ToList();

            var totalCost = cartItemsDTO.Sum(ci => ci.BasePrice * ci.CartItemStock);

            return new PublicCartItemsDTO { CartItems = cartItemsDTO, TotalCost = totalCost };
        }
    }
}
