using Swashbuckle.AspNetCore.Filters;
using WebUser.features.CartItem.DTO;

namespace WebUser.features.Cart.DTO
{
    public class CartDTOSW : IExamplesProvider<CartDTO>
    {
        public CartDTO GetExamples() =>
            new()
            {
                ID = 5,
                Items = new List<CartItemDTO>
                {
                    new()
                    {
                        ID = 1,
                        Amount = 5,

                        ProductID = 3,
                        ProductName = "pen",
                        ProductBasePrice = 10,
                    },
                    new()
                    {
                        ID = 3,
                        Amount = 1,
                        ProductID = 1,
                        ProductName = "cup",
                        ProductBasePrice = 100,
                    },
                },
            };
    };
}
