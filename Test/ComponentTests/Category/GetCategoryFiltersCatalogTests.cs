namespace Test.ComponentTests.Category;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Test.mocks;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.Category.Exceptions;
using WebUser.shared.RepoWrapper;
using static WebUser.features.Category.Functions.GetCategoryFiltersCatalog;

public class GetCategoryFiltersCatalogTests
{
    private Mock<IServiceWrapper> serviceWrapper;

    public GetCategoryFiltersCatalogTests()
    {
        serviceWrapper = ServiceWrapperMock.CreateMock();
    }

    [Fact]
    public async Task ReturnsParentCategoryAttributes_WhenIncludeChildCategoriesFalse()
    {
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);

        // ARRANGE
        var category = new Category
        {
            ID = 1,
            Name = "Category 1",
            Attributes = new List<AttributeNameCategory>
                {
                    new AttributeNameCategory
                    {
                        AttributeName = new AttributeName
                        {
                            Name = "Color",
                            AttributeValues = new List<AttributeValue>
                            {
                                new AttributeValue { Value = "Red" },
                                new AttributeValue { Value = "Blue" },
                            },
                        },
                    },
                },
        };
        dbcontext.Categories.Add(category);
        await dbcontext.SaveChangesAsync();

        var handler = new Handler(dbcontext, serviceWrapper.Object);
        var request = new GetCategoryFiltersCatalogQuery { Id = 1, IncludeChildCategories = false };

        // ACT
        var result = await handler.Handle(request, CancellationToken.None);

        // ASSSERT
        Assert.NotNull(result);
        Assert.Equal(1, result.Attributes.Count);
        Assert.Equal("Color", result.Attributes.First().AttributeName.Name);
        Assert.False(result.IncludesChildCategories);

    }

    [Fact]
    public async Task ReturnsChildCategoryAttributes_WhenIncludeChildCategoriesIsTrue()
    {
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();

        using (var dbcontext = new DB_Context(dbOption))
        {
            //ARRANGE
            serviceWrapper.Setup(s => s.Category.GetAllGenChildCategories(It.IsAny<int>()))
                .ReturnsAsync(new List<Category> { new Category { ID = 2, Name = "Subcategory 1" } });

            var category = new Category
            {
                ID = 1,
                Name = "Category 1",
                Attributes = new List<AttributeNameCategory>
                {
                    new AttributeNameCategory
                    {
                        AttributeName = new AttributeName
                        {
                            Name = "Color",
                            AttributeValues = new List<AttributeValue>
                            {
                                new AttributeValue { Value = "Red" },
                                new AttributeValue { Value = "Blue" },
                            },
                        },
                    },
                },
                Subcategories = new List<Category>
                {
                    new Category
                    {
                        ID = 2,
                        Name = "Subcategory 1",
                        Attributes = new List<AttributeNameCategory>
                        {
                            new AttributeNameCategory
                            {
                                AttributeName = new AttributeName
                                {
                                    Name = "Size",
                                    AttributeValues = new List<AttributeValue>
                                    {
                                        new AttributeValue { Value = "Small" },
                                        new AttributeValue { Value = "Large" },
                                    },
                                },
                            },
                        },
                    },
                },
            };
            dbcontext.Categories.Add(category);
            await dbcontext.SaveChangesAsync();

            var handler = new Handler(dbcontext, serviceWrapper.Object);
            var request = new GetCategoryFiltersCatalogQuery { Id = 1, IncludeChildCategories = true };

            //ACT
            var result = await handler.Handle(request, CancellationToken.None);

            //ASSERT
            Assert.NotNull(result);
            Assert.Equal(2, result.Attributes.Count);
            Assert.True(result.IncludesChildCategories);
            Assert.Contains(result.Attributes, a => a.AttributeName.Name == "Color");
            Assert.Contains(result.Attributes, a => a.AttributeName.Name == "Size");
        }
    }

    [Fact]
    public async Task ReturnsParentAttributes_WhenIncludeChildCategoriesIsTrueAndNoChildCategoriesAttributes()
    {
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);

        //ARRANGE
        serviceWrapper.Setup(s => s.Category.GetAllGenChildCategories(It.IsAny<int>()))
            .ReturnsAsync(new List<Category> { new Category { ID = 2, Name = "Subcategory 1" } });

        var category = new Category
        {
            ID = 1,
            Name = "Category 1",
            Attributes = new List<AttributeNameCategory>
                {
                    new AttributeNameCategory
                    {
                        AttributeName = new AttributeName
                        {
                            Name = "Color",
                            AttributeValues = new List<AttributeValue>
                            {
                                new AttributeValue { Value = "Red" },
                                new AttributeValue { Value = "Blue" },
                            },
                        },
                    },
                },
            Subcategories = new List<Category>
                {
                    new Category
                    {
                        ID = 2,
                        Name = "Subcategory 1",

                    },
                },
        };
        dbcontext.Categories.Add(category);
        await dbcontext.SaveChangesAsync();

        var handler = new Handler(dbcontext, serviceWrapper.Object);
        var request = new GetCategoryFiltersCatalogQuery { Id = 1, IncludeChildCategories = true };

        //ACT
        var result = await handler.Handle(request, CancellationToken.None);

        //ASSERT
        Assert.NotNull(result);
        Assert.Equal(1, result.Attributes.Count);
        Assert.True(result.IncludesChildCategories);
        Assert.Contains(result.Attributes, a => a.AttributeName.Name == "Color");

    }

    [Fact]
    public async Task ThrowsCategoryNotFoundException_WhenDoesNotExist()
    {
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();

        var dbcontext = new DB_Context(dbOption);

        // Arrange
        var mockServiceWrapper = new Mock<IServiceWrapper>();
        var handler = new Handler(dbcontext, mockServiceWrapper.Object);
        var request = new GetCategoryFiltersCatalogQuery { Id = 999, IncludeChildCategories = false };

        // Act & Assert
        await Assert.ThrowsAsync<CategoryNotFoundException>(() => handler.Handle(request, CancellationToken.None));

    }
}
