using Swashbuckle.AspNetCore.Filters;
using WebUser.features.CartItem.DTO;
using WebUser.features.Product.DTO;

namespace WebUser.features.Cart.DTO
{
    public class CartDTOSW : IExamplesProvider<CartDTO>
    {
        public CartDTO GetExamples() =>
            new CartDTO
            {
                ID = 5,
                Items = new List<CartItemDTO>
                {
                    new CartItemDTO
                    {
                        ID = 1,
                        Amount = 5,
                        ProductMin = new ProductMinDTO
                        {
                            ID = 3,
                            Name = "pen",
                            Price = 10
                        }
                    },
                    new CartItemDTO
                    {
                        ID = 3,
                        Amount = 1,
                        ProductMin = new ProductMinDTO
                        {
                            ID = 1,
                            Name = "cup",
                            Price = 100
                        }
                    }
                }
            };
    }
}
