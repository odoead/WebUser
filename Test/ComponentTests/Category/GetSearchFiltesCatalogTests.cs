namespace Test.ComponentTests.Category;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUser.Data;
using WebUser.Domain.entities;
using static WebUser.features.Category.Functions.GetSearchFiltesCatalog;

public class GetSearchFiltesCatalogTests
{
    [Fact]
    public async Task Handle_NoMatchingProducts_ReturnsEmptyResults()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var handler = new Handler(dbcontext);
        var query = new GetSearchFiltesCatalogQuery { RequestName = "NonExistentProduct" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result.Attributes);
        Assert.Empty(result.СategoriesOfFoundItems);
    }

    [Fact]
    public async Task Handle_ReturnRightAttributeNamesValuesAndCategories()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var attributeName1 = new AttributeName { ID = 1, Name = "Color" };
        var category1 = new Category { ID = 1, Name = "Electronics" };
        var category2 = new Category { ID = 2, Name = "Home" };
        var attributeValue1 = new AttributeValue
        {
            ID = 1,
            Value = "Red",
            AttributeName = attributeName1,
        };
        var attributeValue2 = new AttributeValue
        {
            ID = 2,
            Value = "Blue",
            AttributeName = attributeName1,
        };
        attributeName1.AttributeValues = new List<AttributeValue> { attributeValue1, attributeValue2 };
        attributeName1.Categories = new List<AttributeNameCategory>
        {
            new AttributeNameCategory { Category = category1 },
            new AttributeNameCategory { Category = category2 },
        };
        var product1 = new Product
        {
            ID = 1,
            Name = "Product1",
            Description = "Test description",
            AttributeValues = new List<ProductAttributeValue>
            {
                new ProductAttributeValue { AttributeValue = attributeValue1 },
                new ProductAttributeValue { AttributeValue = attributeValue2 },
            },
        };

        dbcontext.Products.Add(product1);
        await dbcontext.SaveChangesAsync();

        var request = new GetSearchFiltesCatalogQuery { RequestName = "Product1" };
        var handler = new Handler(dbcontext);
        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Single(result.Attributes);
        Assert.Equal("Color", result.Attributes.First().AttributeName.Name);
        Assert.Equal(2, result.Attributes.First().Attributes.Count);
        Assert.Contains(result.Attributes.First().Attributes, av => av.Value == "Red");
        Assert.Contains(result.Attributes.First().Attributes, av => av.Value == "Blue");
        Assert.Equal(2, result.СategoriesOfFoundItems.Count);
    }

    [Fact]
    public async Task Handle_ReturnsUniqueCategories()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var category1 = new Category { ID = 1, Name = "Electronics" };
        var category2 = new Category { ID = 2, Name = "Home" };
        var attributeName1 = new AttributeName
        {
            ID = 1,
            Name = "Material",
            Categories = new List<AttributeNameCategory>
            {
                new AttributeNameCategory { Category = category1 },
                new AttributeNameCategory { Category = category2 },
            },
        };
        var attributeValue1 = new AttributeValue
        {
            ID = 1,
            Value = "Metal",
            AttributeName = attributeName1,
        };

        var product1 = new Product
        {
            ID = 1,
            Name = "Product1",
            Description = "Test description",
            AttributeValues = new List<ProductAttributeValue> { new ProductAttributeValue { AttributeValue = attributeValue1 } },
        };

        dbcontext.Products.Add(product1);
        await dbcontext.SaveChangesAsync();

        var request = new GetSearchFiltesCatalogQuery { RequestName = "Product1" };
        var handler = new Handler(dbcontext);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.СategoriesOfFoundItems.Count);
        Assert.Contains(result.СategoriesOfFoundItems, c => c.Name == "Electronics");
        Assert.Contains(result.СategoriesOfFoundItems, c => c.Name == "Home");
    }

    [Fact]
    public async Task Handle_NoMatchingProducts_ReturnsEmptyResult()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var request = new GetSearchFiltesCatalogQuery { RequestName = "NonExistentProduct" };
        var handler = new Handler(dbcontext);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Empty(result.Attributes);
        Assert.Empty(result.СategoriesOfFoundItems);
    }
}
