using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebUser.Domain.entities;

namespace WebUser.Data
{
    public class DB_Context : IdentityDbContext<User>
    {
        public DB_Context() { }

        public DB_Context(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<PromotionAttrValue>().HasKey(pav => new { pav.PromotionID, pav.AttributeValueID });
            builder
                .Entity<PromotionAttrValue>()
                .HasOne(pav => pav.Promotion)
                .WithMany(p => p.AttributeValues)
                .HasForeignKey(pav => pav.PromotionID)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<PromotionAttrValue>()
                .HasOne(pav => pav.AttributeValue)
                .WithMany(av => av.Promotions)
                .HasForeignKey(pav => pav.AttributeValueID)
                .OnDelete(DeleteBehavior.Cascade);
            //------
            builder.Entity<PromotionCategory>().HasKey(pav => new { pav.PromotionID, pav.CategoryID });
            builder
                .Entity<PromotionCategory>()
                .HasOne(pav => pav.Promotion)
                .WithMany(p => p.Categories)
                .HasForeignKey(pav => pav.PromotionID)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<PromotionCategory>()
                .HasOne(pav => pav.Category)
                .WithMany(av => av.Promotions)
                .HasForeignKey(pav => pav.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);
            //---
            builder.Entity<PromotionProduct>().HasKey(pav => new { pav.PromotionID, pav.ProductID });
            builder
                .Entity<PromotionProduct>()
                .HasOne(pav => pav.Promotion)
                .WithMany(p => p.Products)
                .HasForeignKey(pav => pav.PromotionID)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<PromotionProduct>()
                .HasOne(pav => pav.Product)
                .WithMany(av => av.Promotions)
                .HasForeignKey(pav => pav.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
            //---
            builder.Entity<PromotionPromProduct>().HasKey(pav => new { pav.PromotionID, pav.ProductID });
            builder
                .Entity<PromotionPromProduct>()
                .HasOne(pav => pav.Promotion)
                .WithMany(p => p.PromProducts)
                .HasForeignKey(pav => pav.PromotionID)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<PromotionPromProduct>()
                .HasOne(pav => pav.Product)
                .WithMany(av => av.PromoPromotions)
                .HasForeignKey(pav => pav.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
            //---
            builder.Entity<ProductAttributeValue>().HasKey(pav => new { pav.AttributeValueID, pav.ProductID });
            builder
                .Entity<ProductAttributeValue>()
                .HasOne(pav => pav.Product)
                .WithMany(p => p.AttributeValues)
                .HasForeignKey(pav => pav.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<ProductAttributeValue>()
                .HasOne(pav => pav.AttributeValue)
                .WithMany(av => av.Products)
                .HasForeignKey(pav => pav.AttributeValueID)
                .OnDelete(DeleteBehavior.Cascade);
            //------
            builder.Entity<AttributeNameCategory>().HasKey(pav => new { pav.AttributeNameID, pav.CategoryID });
            builder
                .Entity<AttributeNameCategory>()
                .HasOne(pav => pav.Category)
                .WithMany(p => p.Attributes)
                .HasForeignKey(pav => pav.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<AttributeNameCategory>()
                .HasOne(pav => pav.AttributeName)
                .WithMany(av => av.Categories)
                .HasForeignKey(pav => pav.AttributeNameID)
                .OnDelete(DeleteBehavior.Cascade);
            //------- cascade delete of revieews after product deleting
            builder
                .Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
            //--------
            builder.Entity<ProductUserNotificationRequest>().HasKey(pav => new { pav.ProductID, pav.UserID });
            builder
                .Entity<ProductUserNotificationRequest>()
                .HasOne(pav => pav.Product)
                .WithMany(p => p.RequestNotifications)
                .HasForeignKey(pav => pav.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<ProductUserNotificationRequest>()
                .HasOne(pav => pav.User)
                .WithMany(av => av.RequestNotifications)
                .HasForeignKey(pav => pav.UserID)
                .OnDelete(DeleteBehavior.Cascade);
            /*modelBuilder
                .Entity<Promotion>()
                .HasMany(c => c.AttributeValues)
                .WithMany(s => s.Promotions)
                .UsingEntity(j => j.ToTable("PromotionAttrValue"));
            modelBuilder
                .Entity<Promotion>()
                .HasMany(c => c.Categories)
                .WithMany(s => s.Promotions)
                .UsingEntity(j => j.ToTable("PromotionCategory"));
            modelBuilder.Entity<Promotion>(w =>
            {
                w.HasMany<Product>(c => c.Products).WithMany(s => s.Promotions);
                w.ToTable("PromotionProduct");
            });
            modelBuilder.Entity<Promotion>(w =>
            {
                w.HasMany<Product>(c => c.PromoProducts).WithMany(s => s.PromoPromotions);
                w.ToTable("PromotionPromProduct");
            });*/
            /*modelBuilder.Entity<Promotion>()
                .HasMany(c => c.PromoProducts)
                .WithMany(s => s.Promotions)
                .UsingEntity(j => j.ToTable("PromotionPromProduct"));*/
        }

        public DbSet<AttributeName> AttributeNames { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Image> Img { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionProduct> PromotionProducts { get; set; }
        public DbSet<PromotionAttrValue> PromotionAttributeValues { get; set; }
        public DbSet<PromotionCategory> PromotionCategories { get; set; }
        public DbSet<PromotionPromProduct> PromotionPromProducts { get; set; }
        public DbSet<AttributeNameCategory> AttributeNameCategories { get; set; }
        public DbSet<Review> ProductReviews { get; set; }
        public DbSet<ProductUserNotificationRequest> RequestNotifications { get; set; }
    }
}
