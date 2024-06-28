namespace WebUser.Data.Seed;

using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebUser.Domain.entities;
using WebUser.shared.Shared_data;

public static class Seeder
{
    public static async Task Seed(DB_Context db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        /*var tables = db.GetType()
            .GetProperties()
            .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));
        foreach (var item in tables)
        {
            var dbSet = (IQueryable<object>)item.GetValue(db);
            var isEmpty = !dbSet.Any(); //if table is empty
            switch (item.Name)
            {
                case "Users":
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
                    break;
                case "AttributeNameCategories":
                    if (isEmpty)
                    {
                        var AttributeNameCategories = new List<AttributeNameCategory>();
                        foreach (var category in db.Categories.ToList())
                        {
                            var AttributeNameCategoryPairs = db
                                .AttributeNames.OrderBy(x => Guid.NewGuid()) // Randomize attribute values
                                .Take(new Random().Next(1, 5)) // Assign random number of attributes to each product
                                .Select(AttributeName => new AttributeNameCategory { AttributeName = AttributeName, Category = category });
                            AttributeNameCategories.AddRange(AttributeNameCategoryPairs);
                        }
                        await db.AttributeNameCategories.AddRangeAsync(AttributeNameCategories);
                    }
                    break;
                case "Discounts":
                    if (isEmpty)
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
                    break;
                case "Carts":
                    if (isEmpty)
                    {
                        var faker = new Faker<Cart>().RuleFor(q => q.User, f => f.PickRandom(db.Users.ToList()));
                        var data = faker.Generate(4);
                        db.Carts.AddRangeAsync(data);
                    }
                    break;
                case "CartItems": //todo
                    if (isEmpty)
                    {
                        var faker = new Faker<CartItem>()
                            .RuleFor(c => c.Amount, 1)
                            .RuleFor(q => q.Product, f => f.PickRandom(db.Products.ToList()))
                            .RuleFor(q => q.Cart, f => f.PickRandom(db.Carts.ToList()));
                        var data = faker.Generate(4);
                        await db.CartItems.AddRangeAsync(data);
                    }
                    break;
                case "Img":
                    if (isEmpty)
                    {
                        var faker = new Faker<Image>()
                            .RuleFor(i => i.ImageContent, f => GetRandomImageBytes())
                            .RuleFor(q => q.Product, f => f.PickRandom(db.Products.ToList()));
                        var data = faker.Generate(50);
                        await db.Img.AddRangeAsync(data);
                    }
                    break;
                case "Categories":
                    if (isEmpty)
                    {
                        var faker = new Faker<Category>()
                        //.RuleFor(p => p.ID, f => f.IndexVariable++)
                        .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);
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
                    break;
                case "AttributeNames":
                    if (isEmpty)
                    {
                        var faker = new Faker<AttributeName>().RuleFor(a => a.Name, f => f.Commerce.Categories(1)[0]);
                        var data = faker.Generate(20);
                        await db.AttributeNames.AddRangeAsync(data);
                    }
                    break;
                case "AttributeValues":
                    if (isEmpty)
                    {
                        var faker = new Faker<AttributeValue>()
                            .RuleFor(c => c.Value, f => f.Commerce.ProductMaterial())
                            .RuleFor(q => q.AttributeName, f => f.PickRandom(db.AttributeNames.ToList()));
                        var data = faker.Generate(100);
                        await db.AttributeValues.AddRangeAsync(data);
                    }
                    break;
                case "ProductAttributeValues":
                    if (isEmpty)
                    {
                        var productAttributeValues = new List<ProductAttributeValue>();
                        foreach (var product in db.Products.ToList())
                        {
                            var productAttributePairs = db
                                .AttributeValues.OrderBy(x => Guid.NewGuid()) // Randomize attribute values
                                .Take(new Random().Next(1, 5)) // Assign random number of attributes to each product
                                .Select(attributeValue => new ProductAttributeValue { Product = product, AttributeValue = attributeValue });
                            productAttributeValues.AddRange(productAttributePairs);
                        }
                        await db.ProductAttributeValues.AddRangeAsync(productAttributeValues);
                    }
                    break;
                case "Products":
                    if (isEmpty)
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
                    break;
            }
        }*/
        db.SaveChangesAsync();
    }

    private static byte[] GetRandomImageBytes()
    {
        // Generate random bytes (replace this with actual logic to generate image bytes)
        var random = new Random();
        byte[] buffer = new byte[1024];
        random.NextBytes(buffer);
        return buffer;
    }

    private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
    {
        var admin = new IdentityRole { Name = "Admin" };
        var user = new IdentityRole { Name = "User" };
        roleManager.CreateAsync(admin);
        roleManager.CreateAsync(user);
    }

    private static async Task CreateAdmin(UserManager<User> userManager)
    {
        var admin = new User { UserName = "admin.localhost", Email = "admin@localhost.com", };
        var result = await userManager.CreateAsync(admin, "1q2w3e4rA!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
/*// Seed Categories
var categories = SeedCategories();
_context.Categories.AddRange(categories);
        _context.SaveChanges();
        // Seed AttributeNames
        var attributeNames = SeedAttributeNames();
_context.AttributeNames.AddRange(attributeNames);
        _context.SaveChanges();
        // Seed AttributeValues
        var attributeValues = SeedAttributeValues(attributeNames);
_context.AttributeValues.AddRange(attributeValues);
        _context.SaveChanges();
        // Seed Products
        var products = SeedProducts(categories);
_context.Products.AddRange(products);
        _context.SaveChanges();
        // Seed ProductAttributeValues
        var productAttributeValues = SeedProductAttributeValues(products, attributeValues);
_context.ProductAttributeValues.AddRange(productAttributeValues);
        _context.SaveChanges();
    }
    private List<Category> SeedCategories()
{
    var categoryFaker = new Faker<Category>()
        .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);
    return categoryFaker.Generate(5); // Generating 5 categories for example
}
private List<AttributeName> SeedAttributeNames()
{
    var attributeNameFaker = new Faker<AttributeName>()
        .RuleFor(a => a.Name, f => f.Commerce.ProductMaterial());
    return attributeNameFaker.Generate(10); // Generating 10 attribute names for example
}
private List<AttributeValue> SeedAttributeValues(List<AttributeName> attributeNames)
{
    var attributeValueFaker = new Faker<AttributeValue>()
        .RuleFor(a => a.Value, f => f.Commerce.ProductAdjective())
        .RuleFor(a => a.AttributeName, f => f.PickRandom(attributeNames));
    return attributeValueFaker.Generate(50); // Generating 50 attribute values for example
}
private List<Product> SeedProducts(List<Category> categories)
{
    var productFaker = new Faker<Product>()
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => f.Random.Double(1, 100))
        .RuleFor(p => p.Stock, f => f.Random.Number(0, 100))
        .RuleFor(p => p.DateCreated, f => f.Date.Recent())
        .RuleFor(p => p.Category, f => f.PickRandom(categories));
    return productFaker.Generate(20); // Generating 20 products for example
}
private List<ProductAttributeValue> SeedProductAttributeValues(List<Product> products, List<AttributeValue> attributeValues)
{
    var productAttributeValues = new List<ProductAttributeValue>();
    foreach (var product in products)
    {
        var productAttributePairs = attributeValues
            .OrderBy(x => Guid.NewGuid()) // Randomize attribute values
            .Take(new Random().Next(1, 5)) // Assign random number of attributes to each product
            .Select(attributeValue => new ProductAttributeValue
            {
                Product = product,
                AttributeValue = attributeValue
            });
        productAttributeValues.AddRange(productAttributePairs);
    }
    return productAttributeValues;
}
*/
