using WebUser.Data;
using WebUser.features.Cart.Interfaces;

namespace WebUser.features.Cart
{
    public class CartService : ICartService
    {
        private readonly DB_Context _Context;

        public CartService(DB_Context context)
        {
            _Context = context;
        }
    }
}
