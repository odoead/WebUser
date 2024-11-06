namespace Test.ComponentTests.Category;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.features.Category.Functions;

public class UpdateCategoryTests
{



    [Fact]
    public async Task CategoryNotFound_ThrowsCategoryNotFoundException()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var _handler = new UpdateCategory.Handler(_dbContext);
        var command = new UpdateCategory.UpdateCategoryCommand { Id = 1, PatchDoc = new JsonPatchDocument<UpdateCategoryDTO>() };
        // Act & Assert
        await Assert.ThrowsAsync<CategoryNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task UpdatesCategory()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbContext = new DB_Context(dbOption);
        var _handler = new UpdateCategory.Handler(dbContext);
        var category = new Category { ID = 1, Name = "Test Category" };
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var patchDoc = new JsonPatchDocument<UpdateCategoryDTO>();
        patchDoc.Replace(c => c.Name, "Updated Category");
        var command = new UpdateCategory.UpdateCategoryCommand { Id = 1, PatchDoc = patchDoc };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedCategory = await dbContext.Categories.FindAsync(1);
        Assert.Equal("Updated Category", updatedCategory.Name);
    }

    [Fact]
    public async Task UpdatesCategory_EditsNewAttributes()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbContext = new DB_Context(dbOption);
        var _handler = new UpdateCategory.Handler(dbContext);
        var category = new Category { ID = 1, Name = "Test Category" };
        var attribute = new AttributeName
        {
            ID = 1,
            Description = "Test Attribute 1",
            Name = "Test Attribute Name 1",
        };
        var startAttribute = new AttributeName
        {
            ID = 999,
            Description = "Test Attribute 999",
            Name = "Test Attribute Name 999",
        };
        category.Attributes = new List<AttributeNameCategory>
        {
            new()
            {
                AttributeName = startAttribute,
                AttributeNameID = startAttribute.ID,
                Category = category,
                CategoryID = category.ID,
            },
        };
        dbContext.Categories.Add(category);
        dbContext.AttributeNames.Add(attribute);
        await dbContext.SaveChangesAsync();

        var patchDoc = new JsonPatchDocument<UpdateCategoryDTO>();
        patchDoc.Replace(q => q.AttributeNameIds, 1);
        var command = new UpdateCategory.UpdateCategoryCommand { Id = 1, PatchDoc = patchDoc };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedCategory = await dbContext.Categories.Include(q => q.Attributes).FirstOrDefaultAsync(q => q.ID == 1);
        Assert.Equal(1, updatedCategory.Attributes.Count);
        var resultAttributeName = new List<AttributeNameCategory>();
        Assert.Contains(updatedCategory.Attributes, a => a.AttributeNameID == 1);

        Assert.Equal(1, await dbContext.AttributeNameCategories.CountAsync());
        Assert.Equal(attribute.ID, await dbContext.AttributeNameCategories.Select(q => q.AttributeNameID).FirstOrDefaultAsync());
    }

    [Fact]
    public async Task UpdatesCategory_EditsSubcategories()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbContext = new DB_Context(dbOption);
        var _handler = new UpdateCategory.Handler(dbContext);
        var subcategories = new List<Category>
        {
            new Category { ID = 1, Name = "Test Category 1" },
            new Category { ID = 2, Name = "Test Category 2" },
            new Category { ID = 3, Name = "Test Category 3" },
        };
        var category = new Category
        {
            ID = 4,
            Name = "Test Category 4",
            Subcategories = subcategories,
        };

        await dbContext.Categories.AddRangeAsync(subcategories);
        await dbContext.Categories.AddAsync(category);
        await dbContext.SaveChangesAsync();

        var patchDoc = new JsonPatchDocument<UpdateCategoryDTO>();
        patchDoc.Replace(q => q.SubcategoryIds, new List<int> { 1, 2 });
        var command = new UpdateCategory.UpdateCategoryCommand { Id = 1, PatchDoc = patchDoc };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedCategory = await dbContext.Categories.Include(q => q.Subcategories).FirstOrDefaultAsync(q => q.ID == 1);
        Assert.Equal(2, updatedCategory.Subcategories.Count);
        var expectedSubcategories = await dbContext.Categories.Where(q => q.ID == 1 || q.ID == 2).ToListAsync();
        Assert.Equal(expectedSubcategories, updatedCategory.Subcategories);
    }

    [Fact]
    public async Task UpdatesCategory_EditsParentcategory()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbContext = new DB_Context(dbOption);
        var _handler = new UpdateCategory.Handler(dbContext);
        var parent = new Category { ID = 1, Name = "Test Category 1" };
        var category1 = new Category { ID = 2, Name = "Test Category 2" };

        var category = new Category
        {
            ID = 4,
            Name = "Test Category 4",
            ParentCategory = parent,
        };

        await dbContext.Categories.AddRangeAsync(parent, category, category1);
        await dbContext.SaveChangesAsync();

        var patchDoc = new JsonPatchDocument<UpdateCategoryDTO>();
        patchDoc.Replace(q => q.ParentCategoryId, 2);
        var command = new UpdateCategory.UpdateCategoryCommand { Id = 1, PatchDoc = patchDoc };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedCategory = await dbContext.Categories.Include(q => q.Subcategories).FirstOrDefaultAsync(q => q.ID == 1);
        Assert.Equal(2, updatedCategory.ParentCategoryID);
        Assert.Equal(new List<int> { 1 }, updatedCategory.ParentCategory.Subcategories.Select(q => q.ID));
    }
}
