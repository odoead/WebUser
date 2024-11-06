namespace Test.ComponentTests.Product;

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.Product.extensions;
using WebUser.PricingService.DTO;
using WebUser.shared.RepoWrapper;
using static WebUser.features.Product.Functions.GetAllProductsThumbnail;

public class GetAllProductsThumbnailTests
{
    private Mock<IServiceWrapper> serviceWrapper;

    public GetAllProductsThumbnailTests()
    {
        serviceWrapper = new Mock<IServiceWrapper>();
    }

    [Fact]
    public async Task Handle_ShouldReturnProducts_WhenCategoryIdIsProvided()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var rootCategoryId = 1;
        var subCategoryId = 2;
        var attributeNameId = 1;
        var attributeValueId = 1;

        // Set up categories
        var rootCategory = new Category { ID = rootCategoryId, Name = "RootCategory" };
        var subCategory = new Category
        {
            ID = subCategoryId,
            Name = "SubCategory",
            ParentCategoryID = rootCategoryId,
        };

        // Set up attribute name and values
        var attributeName = new AttributeName
        {
            ID = attributeNameId,
            Name = "Color",
            Categories = new List<AttributeNameCategory>
            {
                new AttributeNameCategory { Category = rootCategory },
                new AttributeNameCategory { Category = subCategory },
            },
        };
        var attributeValue = new AttributeValue
        {
            ID = attributeValueId,
            Value = "Red",
            AttributeName = attributeName,
        };

        // Set up product with attribute values
        var product = new Product
        {
            ID = 1,
            Name = "ProductWithRedColor",
            Description = "Test description",
            Price = 100,
            AttributeValues = new List<ProductAttributeValue>
            {
                new ProductAttributeValue { AttributeValueID = attributeValueId, AttributeValue = attributeValue },
            },
            Images = new List<Image>
            {
                new Image
                {
                    ID = 1,
                    ImageContent = Encoding.ASCII.GetBytes("test-image-1"),
                    Name = "TestIMG",
                },
            },
        };

        dbcontext.Categories.AddRange(rootCategory, subCategory);
        dbcontext.AttributeNames.Add(attributeName);
        dbcontext.AttributeValues.Add(attributeValue);
        dbcontext.Products.Add(product);
        await dbcontext.SaveChangesAsync();

        var query = new GetAllProductsThumbnailQuery
        {
            CategoryId = rootCategoryId,
            IncludeChildCategories = true,
            Parameters = new ProductRequestParameters
            {
                PageNumber = 1,
                PageSize = 5,
                AttributeValueIDs = new List<int> { attributeValueId },
            },
        };

        // Set up the service mock for category fetching
        serviceWrapper.Setup(s => s.Category.GetAllGenChildCategories(rootCategoryId)).ReturnsAsync(new List<Category> { subCategory });
        serviceWrapper.Setup(s => s.Pricing.ApplyDiscountAsync(It.IsAny<List<int>>())).ReturnsAsync(new List<DiscountRecordDTO> { });
        serviceWrapper
            .Setup(s => s.Product.CalculatePriceWithCumulativeDiscounts(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<List<DiscountRecordDTO>>()))
            .Returns((int id, double price, List<DiscountRecordDTO> discounts) => price);

        var handler = new Handler(dbcontext, serviceWrapper.Object);
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.First().Images.Count);
        Assert.Equal(1, result.PagesStat.TotalCount);
        Assert.Equal(5, result.PagesStat.PageSize);
        Assert.Equal(1, result.PagesStat.PageCount);
    }

    public DB_Context SeedDatabase()
    {
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);

        // Create root category
        var rootCategory = new Category { ID = 1, Name = "RootCategory" };
        var subCategory = new Category
        {
            ID = 2,
            Name = "SubCategory",
            ParentCategoryID = 1,
        };

        dbcontext.Categories.AddRange(rootCategory, subCategory);

        // Create attribute names and values
        var attributeName = new AttributeName
        {
            ID = 1,
            Name = "Color",
            Categories = new List<AttributeNameCategory>
            {
                new AttributeNameCategory { CategoryID = 1, Category = rootCategory },
                new AttributeNameCategory { CategoryID = 2, Category = subCategory },
            },
        };

        var attributeValue = new AttributeValue
        {
            ID = 1,
            Value = "Red",
            AttributeName = attributeName,
        };

        dbcontext.AttributeNames.Add(attributeName);
        dbcontext.AttributeValues.Add(attributeValue);

        // Create products
        var products = new List<Product>
        {
            new Product
            {
                ID = 1,
                Name = "Product1",
                Description = "Test description 1",
                Price = 100,
                AttributeValues = new List<ProductAttributeValue>
                {
                    new ProductAttributeValue { AttributeValueID = 1, AttributeValue = attributeValue },
                },
                Images = new List<Image>
                {
                    new Image
                    {
                        ID = 1,
                        ImageContent = Encoding.ASCII.GetBytes("test-image-1"),
                        Name = "test-image-1",
                    },
                },
                Discounts = new List<Discount>
                {
                    new Discount
                    {
                        ID = 1,
                        DiscountVal = 10,
                        DiscountPercent = 0,
                        ActiveFrom = DateTime.Now.AddDays(-1),
                        ActiveTo = DateTime.Now.AddDays(1),
                    },
                },
            },
            new Product
            {
                ID = 2,
                Name = "Product2",
                Description = "Test description 2",
                Price = 200,
                AttributeValues = new List<ProductAttributeValue>
                {
                    new ProductAttributeValue { AttributeValueID = 1, AttributeValue = attributeValue },
                },
                Images = new List<Image>
                {
                    new Image
                    {
                        ID = 2,
                        ImageContent = Encoding.ASCII.GetBytes("test-image-2"),
                        Name = "test-image-2",
                    },
                },
            },
        };

        dbcontext.Products.AddRange(products);
        dbcontext.SaveChanges();
        return dbcontext;
    }

    [Fact]
    public async Task Handle_WithNullCategoryId_ReturnsProductsFromRootCategory()
    {
        // Arrange
        var dbcontext = SeedDatabase();
        var query = new GetAllProductsThumbnailQuery
        {
            Parameters = new ProductRequestParameters { PageNumber = 1, PageSize = 10 },
            CategoryId = null,
        };

        var childCategories = new List<Category> { new Category { ID = 2 } };

        serviceWrapper.Setup(s => s.Category.GetAllGenChildCategories(1)).ReturnsAsync(childCategories);

        serviceWrapper.Setup(s => s.Pricing.ApplyDiscountAsync(It.IsAny<List<int>>())).ReturnsAsync(new List<DiscountRecordDTO>());

        serviceWrapper
            .Setup(s => s.Product.CalculatePriceWithCumulativeDiscounts(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<List<DiscountRecordDTO>>()))
            .Returns((int id, double price, List<DiscountRecordDTO> discounts) => price);
        var handler = new Handler(dbcontext, serviceWrapper.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.PagesStat.PageCount);
        Assert.Equal(2, result.Count);
        Assert.NotNull(result.Where(q => q.Name == "Product1"));
        Assert.NotNull(result.Where(q => q.Name == "Product2"));
    }

    [Fact]
    public async Task Handle_WithSpecificCategory_ReturnsFilteredProducts()
    {
        // Arrange
        var dbcontext = SeedDatabase();
        var query = new GetAllProductsThumbnailQuery
        {
            Parameters = new ProductRequestParameters { PageNumber = 1, PageSize = 10 },
            CategoryId = 2,
            IncludeChildCategories = false,
        };

        serviceWrapper.Setup(s => s.Pricing.ApplyDiscountAsync(It.IsAny<List<int>>())).ReturnsAsync(new List<DiscountRecordDTO>());

        serviceWrapper
            .Setup(s => s.Product.CalculatePriceWithCumulativeDiscounts(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<List<DiscountRecordDTO>>()))
            .Returns((int id, double price, List<DiscountRecordDTO> discounts) => price);

        var handler = new Handler(dbcontext, serviceWrapper.Object);
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.PagesStat.TotalCount);
        Assert.NotNull(result.Where(q => q.Name == "Product1"));
    }

    [Fact]
    public async Task Handle_WithSearchParameters_ReturnsFilteredProducts()
    {
        // Arrange
        var dbcontext = SeedDatabase();
        var query = new GetAllProductsThumbnailQuery
        {
            Parameters = new ProductRequestParameters
            {
                PageNumber = 1,
                PageSize = 10,
                RequestName = "Product1",
                MinPrice = 50,
                MaxPrice = 150,
            },
            CategoryId = 1,
            IncludeChildCategories = true,
        };

        var childCategories = new List<Category> { new Category { ID = 2 } };

        serviceWrapper.Setup(s => s.Category.GetAllGenChildCategories(1)).ReturnsAsync(childCategories);

        serviceWrapper.Setup(s => s.Pricing.ApplyDiscountAsync(It.IsAny<List<int>>())).ReturnsAsync(new List<DiscountRecordDTO>());

        serviceWrapper
            .Setup(s => s.Product.CalculatePriceWithCumulativeDiscounts(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<List<DiscountRecordDTO>>()))
            .Returns((int id, double price, List<DiscountRecordDTO> discounts) => price);

        var handler = new Handler(dbcontext, serviceWrapper.Object);
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.PagesStat.PageCount);
        Assert.Equal(1, result.ToList().Count);
        Assert.NotNull(result.Where(q => q.BasePrice == 100));
    }

    [Fact]
    public async Task Handle_WithDiscounts_CalculatesCorrectPrices()
    {
        // Arrange
        var dbcontext = SeedDatabase();
        var query = new GetAllProductsThumbnailQuery
        {
            Parameters = new ProductRequestParameters { PageNumber = 1, PageSize = 10 },
            CategoryId = 1,
        };

        var discounts = new List<DiscountRecordDTO>
        {
            new DiscountRecordDTO
            {
                ProductId = 1,
                AbsoluteDiscountValue = 10,
                PercentDiscountValue = 0,
                ValueTypes = new List<DiscountValueType> { DiscountValueType.Absolute },
            },
        };

        serviceWrapper.Setup(s => s.Pricing.ApplyDiscountAsync(It.IsAny<List<int>>())).ReturnsAsync(discounts);

        serviceWrapper
            .Setup(s => s.Product.CalculatePriceWithCumulativeDiscounts(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<List<DiscountRecordDTO>>()))
            .Returns((int id, double price, List<DiscountRecordDTO> discounts) => price - 10);

        var handler = new Handler(dbcontext, serviceWrapper.Object);
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var product1 = result.First(p => p.ID == 1);
        Assert.Equal(90, product1.AfterDiscountPrice);
    }
}
