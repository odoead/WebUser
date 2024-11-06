namespace Test.ComponentTests.AttributeName;

using Bogus;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeName.functions;
using WebUser.shared.RequestForming.features;

public class GetAllAttributeNamesTests
{
    [Fact]
    public async Task ExistingList_ShouldReturnPaginatedList()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var attributeValues = new List<AttributeValue>
        {
            new AttributeValue
            {
                ID = 1,
                Value = "Red",
                AttributeNameID = 1,
            },
            new AttributeValue
            {
                ID = 2,
                Value = "Blue",
                AttributeNameID = 1,
            },
            new AttributeValue
            {
                ID = 3,
                Value = "Green",
                AttributeNameID = 1,
            },
            new AttributeValue
            {
                ID = 4,
                Value = "Small",
                AttributeNameID = 2,
            },
            new AttributeValue
            {
                ID = 5,
                Value = "Medium",
                AttributeNameID = 2,
            },
            new AttributeValue
            {
                ID = 6,
                Value = "Large",
                AttributeNameID = 2,
            },
        };

        var attributeNames = new List<AttributeName>
        {
            new()
            {
                ID = 1,
                Name = "Attribute 1",
                Description = "Description 1",
            },
            new()
            {
                ID = 2,
                Name = "Attribute 2",
                Description = "Description 2",
            },
            new()
            {
                ID = 3,
                Name = "Attribute 3",
                Description = "Description 3",
            },
            new()
            {
                ID = 4,
                Name = "Attribute 4",
                Description = "Description 4",
            },
        };
        var attributeNameCategs = new List<AttributeNameCategory>
        {
            new AttributeNameCategory { AttributeNameID = 1, CategoryID = 1 },
            new AttributeNameCategory { AttributeNameID = 1, CategoryID = 2 },
            new AttributeNameCategory { AttributeNameID = 2, CategoryID = 3 },
        };
        await _dbContext.AttributeNames.AddRangeAsync(attributeNames);
        await _dbContext.AttributeValues.AddRangeAsync(attributeValues);
        await _dbContext.AttributeNameCategories.AddRangeAsync(attributeNameCategs);
        await _dbContext.SaveChangesAsync();
        var dtos = new List<AttributeNameDTO>
        {
            new()
            {
                Description = "Description 1",
                Name = "Attribute 1",
                Id = 1,
            },
            new()
            {
                Description = "Description 2",
                Name = "Attribute 2",
                Id = 2,
            },
        };

        var handler = new GetAllAttrNameAsync.Handler(_dbContext);
        var query = new GetAllAttrNameAsync.GetAllAttrNameQuery
        {
            Parameters = new AttributeNameRequestParameters { PageNumber = 1, PageSize = 2 },
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsType<PagedList<AttributeNameDTO>>(result);
        Assert.True(result.PagesStat.IsFirst);
        Assert.False(result.PagesStat.IsLast);
        Assert.True(result.PagesStat.HasNext);
        Assert.False(result.PagesStat.HasPrev);
        Assert.False(result.PagesStat.HasPrev);
        Assert.Equal(4, result.PagesStat.TotalCount);
        Assert.Equal(2, result.PagesStat.PageCount);
        Assert.Equal(2, result.PagesStat.PageSize);
        Assert.Equal(2, result.Count);
    }

    private List<AttributeName> GenerateAttributeNames(int count)
    {
        var faker = new Faker();

        var attributeNames = new List<AttributeName>();
        for (int i = 0; i < count; i++)
        {
            var attributeName = new AttributeName
            {
                ID = i + 1,
                Name = faker.Commerce.ProductMaterial(),
                Description = faker.Lorem.Sentence(),
                AttributeValues = GenerateAttributeValues(faker, 3), // Generates 3 random AttributeValues
                Categories = GenerateAttributeNameCategories(
                    faker,
                    2
                ) // Generates 2 random Categories
                ,
            };
            attributeNames.Add(attributeName);
        }
        return attributeNames;
    }

    private List<AttributeValue> GenerateAttributeValues(Faker faker, int count)
    {
        var attributeValues = new List<AttributeValue>();
        for (int i = 0; i < count; i++)
        {
            var attributeValue = new AttributeValue
            {
                ID = i + 1,
                Value = faker.Commerce.ProductAdjective(),
                AttributeNameID = faker.Random.Int(1, 100),
            };
            attributeValues.Add(attributeValue);
        }
        return attributeValues;
    }

    private List<AttributeNameCategory> GenerateAttributeNameCategories(Faker faker, int count)
    {
        var categories = new List<AttributeNameCategory>();
        for (int i = 0; i < count; i++)
        {
            var category = new AttributeNameCategory { AttributeNameID = faker.Random.Int(1, 100), CategoryID = faker.Random.Int(1, 50) };
            categories.Add(category);
        }
        return categories;
    }
}
