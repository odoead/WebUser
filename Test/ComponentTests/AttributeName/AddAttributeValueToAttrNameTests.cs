namespace Test.ComponentTests.AttributeName;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.Exceptions;
using WebUser.features.AttributeName.functions;
using Xunit;
using static WebUser.features.Category.Functions.GetCategoryFiltersCatalog;

public class AddAttributeValueToAttrNameTests
{
    public AddAttributeValueToAttrNameTests() { }

    [Fact]
    public async Task ExistingAttributeName_NewAttributeValue_AddsNewAttributeValue()
    {
        // ARRANGE
        var command = new AddAttributeValueToAttrName.AddAttributeValueCommand { AttributeValue = "NewValue", AttributeNameID = 1 };
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var attributeName = new AttributeName
        {
            ID = 1,
            Name = "AttributeName",
            AttributeValues = new List<AttributeValue>(),
        };
        _dbContext.AttributeNames.Add(attributeName);
        await _dbContext.SaveChangesAsync();
        var _handler = new AddAttributeValueToAttrName.Handler(_dbContext);
        var request = new GetCategoryFiltersCatalogQuery { Id = 1, IncludeChildCategories = true };

        // ACT
        await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        var updatedAttributeName = await _dbContext.AttributeNames.Include(an => an.AttributeValues).FirstOrDefaultAsync(an => an.ID == 1);

        Assert.NotNull(updatedAttributeName);
        Assert.Single(updatedAttributeName.AttributeValues);
        Assert.Equal("NewValue", updatedAttributeName.AttributeValues.First().Value);
    }

    [Fact]
    public async Task ExistingAttributeName_ExistingAttributeValue_DoesNotAddDuplicate()
    {
        // ARRANGE
        var command = new AddAttributeValueToAttrName.AddAttributeValueCommand { AttributeValue = "ExistingValue", AttributeNameID = 1 };
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var _handler = new AddAttributeValueToAttrName.Handler(_dbContext);
        var attributeName = new AttributeName
        {
            ID = 1,
            Name = "AttributeName",
            AttributeValues = new List<AttributeValue>(),
        };
        var existingAttributeValue = new AttributeValue { AttributeName = attributeName, Value = "ExistingValue" };
        attributeName.AttributeValues.Add(existingAttributeValue);

        _dbContext.AttributeNames.Add(attributeName);
        await _dbContext.SaveChangesAsync();

        // ACT
        await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        var updatedAttributeName = await _dbContext.AttributeNames.Include(an => an.AttributeValues).FirstOrDefaultAsync(an => an.ID == 1);

        Assert.NotNull(updatedAttributeName);
        Assert.Single(updatedAttributeName.AttributeValues);
        Assert.Equal("ExistingValue", updatedAttributeName.AttributeValues.First().Value);
    }

    [Fact]
    public async Task NonExistingAttributeName_ThrowsAttributeNameNotFoundException()
    {
        // ARRANGE
        var command = new AddAttributeValueToAttrName.AddAttributeValueCommand { AttributeValue = "NewValue", AttributeNameID = 999 };
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var _handler = new AddAttributeValueToAttrName.Handler(_dbContext);

        // ACT
        // ASSERT
        await Assert.ThrowsAsync<AttributeNameNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
