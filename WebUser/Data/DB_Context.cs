using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebUser.Domain.entities;

namespace WebUser.Data
{
    public class DB_Context: DbContext
    {

        public DB_Context(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<AttributeName> attributeNames { get; set; }
        public DbSet<AttributeValue> attributeValues { get; set; }
        public DbSet<Cart>carts { get; set; }
        public DbSet<CartItem> cartItems { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Coupon> coupons { get; set; }
        public DbSet<Discount> discounts { get; set; }
        public DbSet<Image> image { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderProduct>orderProducts { get; set; }
        public DbSet<Point> points { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Promotion> promotions { get; set; }
    }
}
