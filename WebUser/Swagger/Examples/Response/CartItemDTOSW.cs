using Swashbuckle.AspNetCore.Filters;
using WebUser.features.Product.DTO;

namespace WebUser.features.CartItem.DTO
{
    public class CartItemDTOSW : IExamplesProvider<CartItemDTO>
    {
        public CartItemDTO GetExamples() =>
            new CartItemDTO
            {
                ID = 1,
                Amount = 10,
                ProductMin = new ProductMinDTO
                {
                    ID = 3,
                    Name = "pen",
                    Price = 10
                },
            };
    }
}
