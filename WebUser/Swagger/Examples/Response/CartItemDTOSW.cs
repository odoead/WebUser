using Swashbuckle.AspNetCore.Filters;

namespace WebUser.features.CartItem.DTO
{
    public class CartItemDTOSW : IExamplesProvider<CartItemDTO>
    {
        public CartItemDTO GetExamples() =>
            new()
            {
                ID = 1,
                Amount = 10,

                ProductID = 3,
                ProductName = "pen",
                ProductBasePrice = 10,

            };
    }
}
