using WebUser.Data;
using WebUser.features.Cart.Interfaces;

namespace WebUser.features.Cart
{
    public class ProductService : IProductService
    {
        private readonly DB_Context dbcontext;

        //private readonly EmailService emailService;

        public ProductService(
            DB_Context context /*, EmailService emailService*/
        )
        {
            dbcontext = context;
            //this.emailService = emailService;
        }

        /*public async Task NotifyUsersAsync(int productId)
        {
            var product = await dbcontext.Products.Include(p => p.Wishlist).ThenInclude(up => up.User).FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                var usersToNotify = await product.Wishlist.Select(up => up.User).Distinct().ToListAsync();

                foreach (var user in usersToNotify)
                {
                    var userWishlistProducts = await user.Wishlist.Where(up => up.Product.Stock > 0).Select(up => up.Product).ToListAsync();

                    var message = $"User {user.Email}, your products from the wishlist became available again!\nProduct list:\n";

                    foreach (var wishProd in userWishlistProducts)
                    {
                        message += $"{wishProd.Name}: {wishProd.Price}\n";
                    }

                    await emailService.SendEmailAsync(user.Email, "Wishlist Product Available", message);
                }
            }
        }*/
    }
}
