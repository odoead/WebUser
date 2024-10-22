namespace WebUser.Data.Seed;

using Bogus;
using Microsoft.AspNetCore.Identity;
using WebUser.Domain.entities;
using WebUser.shared.Shared_data;

public static class Seeder
{
    public static async Task Seed(DB_Context db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Roles and Users
        await SeedRolesAndUsers(db, userManager, roleManager);
        await db.SaveChangesAsync();

        //Seed independent tables first
        await SeedCategories(db);
        await db.SaveChangesAsync();

        await SeedAttributeNames(db);
        await db.SaveChangesAsync();

        await SeedAttributeNameCategories(db);
        await db.SaveChangesAsync();

        await SeedProducts(db);
        await db.SaveChangesAsync();

        //Seed dependent tables after the independent ones
        await SeedAttributeValues(db);
        await db.SaveChangesAsync();

        await SeedProductAttributeValues(db);
        await db.SaveChangesAsync();

        await SeedDiscounts(db);
        await db.SaveChangesAsync();

        await SeedCarts(db);
        await db.SaveChangesAsync();

        await SeedCartItems(db);

        //Save all changes to the database at the end
        await db.SaveChangesAsync();
    }

    // Seed Roles and Users
    private static async Task SeedRolesAndUsers(DB_Context db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        var isEmpty = !db.Users.Any();
        if (isEmpty)
        {
            await CreateRoles(roleManager);
            await CreateAdmin(userManager);

            var faker = new Faker<User>()
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.DateCreated, f => f.Date.Past())
                .RuleFor(u => u.PasswordHash, f => f.Internet.Password())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName));
            var data = faker.Generate(10);
            await db.Users.AddRangeAsync(data);

            foreach (var user in data)
            {
                await userManager.AddToRoleAsync(user, "User");
            }
        }
    }

    // Seed Categories
    private static async Task SeedCategories(DB_Context db)
    {
        if (!db.Categories.Any())
        {
            var faker = new Faker<Category>().RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);
            var categories = faker.Generate(4);

            foreach (var category in categories)
            {
                Random rnd = new Random();
                var subfaker = new Faker<Category>().RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);
                var subs = subfaker.Generate(rnd.Next(0, 2));
                category.Subcategories = subs;
            }

            await db.Categories.AddRangeAsync(categories);
        }
    }

    // Seed AttributeNames
    private static async Task SeedAttributeNames(DB_Context db)
    {
        if (!db.AttributeNames.Any())
        {
            var faker = new Faker<AttributeName>()
                .RuleFor(a => a.Name, f => f.Commerce.Categories(1)[0])
                .RuleFor(a => a.Description, f => f.Commerce.Categories(1)[0]);
            var data = faker.Generate(20);
            await db.AttributeNames.AddRangeAsync(data);
        }
    }

    // Seed Products
    private static async Task SeedProducts(DB_Context db)
    {
        if (!db.Products.Any())
        {
            var faker = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.DateCreated, f => f.Date.Past())
                .RuleFor(p => p.Price, f => f.Random.Double(1, 1000))
                .RuleFor(p => p.Stock, f => f.Random.Number(50, 100))
                .RuleFor(p => p.ReservedStock, f => f.Random.Number(0, 50));
            var data = faker.Generate(20);
            await db.Products.AddRangeAsync(data);
        }
    }

    // Seed AttributeValues (Dependent on AttributeNames)
    private static async Task SeedAttributeValues(DB_Context db)
    {
        if (!db.AttributeValues.Any() && db.AttributeNames.Any())
        {
            var faker = new Faker<AttributeValue>()
                .RuleFor(c => c.Value, f => f.Commerce.ProductMaterial())
                .RuleFor(q => q.AttributeName, f => f.PickRandom(db.AttributeNames.ToList()));
            var data = faker.Generate(100);
            await db.AttributeValues.AddRangeAsync(data);
        }
    }

    private static async Task SeedAttributeNameCategories(DB_Context db)
    {
        if (!db.AttributeNameCategories.Any() && db.AttributeNames.Any() && db.Categories.Any())
        {
            var AttributeNameCategories = new List<AttributeNameCategory>();
            foreach (var category in db.Categories.ToList())
            {
                var AttributeNameCategoryPairs = db
                    .AttributeNames.OrderBy(x => Guid.NewGuid())
                    .Take(new Random().Next(1, 5))
                    .Select(AttributeName => new AttributeNameCategory { Category = category, AttributeName = AttributeName });
                AttributeNameCategories.AddRange(AttributeNameCategoryPairs);
            }

            await db.AttributeNameCategories.AddRangeAsync(AttributeNameCategories);
        }
    }

    // Seed ProductAttributeValues (Dependent on Products and AttributeValues)
    private static async Task SeedProductAttributeValues(DB_Context db)
    {
        if (!db.ProductAttributeValues.Any() && db.Products.Any() && db.AttributeValues.Any())
        {
            var productAttributeValues = new List<ProductAttributeValue>();

            foreach (var product in db.Products.ToList())
            {
                var productAttributePairs = db
                    .AttributeValues.OrderBy(x => Guid.NewGuid())
                    .Take(new Random().Next(1, 5))
                    .Select(attributeValue => new ProductAttributeValue { Product = product, AttributeValue = attributeValue });
                productAttributeValues.AddRange(productAttributePairs);
            }

            await db.ProductAttributeValues.AddRangeAsync(productAttributeValues);
        }
    }

    // Seed Discounts (Dependent on Products)
    private static async Task SeedDiscounts(DB_Context db)
    {
        if (!db.Discounts.Any() && db.Products.Any())
        {
            var faker = new Faker<Discount>()
                .RuleFor(d => d.CreatedAt, f => f.Date.Past())
                .RuleFor(d => d.ActiveFrom, f => f.Date.Recent())
                .RuleFor(d => d.ActiveTo, (f, d) => f.Date.Future(1, d.ActiveFrom))
                .RuleFor(d => d.DiscountVal, f => f.Random.Double(1, 400))
                .RuleFor(d => d.DiscountPercent, f => f.Random.Int(1, 100))
                .RuleFor(q => q.Product, f => f.PickRandom(db.Products.ToList()));
            var data = faker.Generate(4);
            await db.Discounts.AddRangeAsync(data);
        }
    }

    // Seed Carts (Dependent on Users)
    private static async Task SeedCarts(DB_Context db)
    {
        if (!db.Carts.Any() && db.Users.Any())
        {
            var faker = new Faker<Cart>().RuleFor(q => q.User, f => f.PickRandom(db.Users.ToList()));
            var data = faker.Generate(4);
            await db.Carts.AddRangeAsync(data);
        }
    }

    // Seed CartItems (Dependent on Products and Carts)
    private static async Task SeedCartItems(DB_Context db)
    {
        if (!db.CartItems.Any() && db.Products.Any() && db.Carts.Any())
        {
            var faker = new Faker<CartItem>()
                .RuleFor(c => c.Amount, 1)
                .RuleFor(q => q.Product, f => f.PickRandom(db.Products.ToList()))
                .RuleFor(q => q.Cart, f => f.PickRandom(db.Carts.ToList()));
            var data = faker.Generate(4);
            await db.CartItems.AddRangeAsync(data);
        }
    }

    // Create roles
    private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
    {
        var admin = new IdentityRole { Name = Roles.Admin.ToString() };
        var user = new IdentityRole { Name = Roles.User.ToString() };
        await roleManager.CreateAsync(admin);
        await roleManager.CreateAsync(user);
    }

    // Create admin user
    private static async Task CreateAdmin(UserManager<User> userManager)
    {
        var admin = new User
        {
            UserName = "admin.localhost",
            Email = "admin@localhost.com",
            FirstName = "Kirill",
            LastName = "Muraviov",
        };

        var result = await userManager.CreateAsync(admin, "1q2w3e4rA!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
