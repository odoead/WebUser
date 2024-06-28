using WebUser.Data;
using WebUser.features.Cart.Interfaces;

namespace WebUser.features.Cart
{
    public class CartItemService : ICartItemService
    {
        private readonly DB_Context _Context;

        public CartItemService(DB_Context context)
        {
            _Context = context;
        }
    }
}
