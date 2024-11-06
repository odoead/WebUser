namespace Test.ComponentTests.AttributeName;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.Exceptions;
using WebUser.features.AttributeName.functions;

public class RemoveAttributeValueFromAttrNameTests
{


    [Fact]
    public async Task NonExistingAttributeName_ThrowsAttributeNameNotFoundException()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var command = new RemoveAttributeValueFromAttrName.DeleteAttributeValueCommand { AttributeNameId = 999, AttributeValueId = 1 };
        var _handler = new RemoveAttributeValueFromAttrName.Handler(_dbContext);

        // Act & Assert
        await Assert.ThrowsAsync<AttributeNameNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task NonExistingAttributeValue_DoesNotThrowException()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var _handler = new RemoveAttributeValueFromAttrName.Handler(_dbContext);

        var attributeName = new AttributeName { ID = 1, Name = "TestAttribute" };
        _dbContext.AttributeNames.Add(attributeName);
        await _dbContext.SaveChangesAsync();

        var command = new RemoveAttributeValueFromAttrName.DeleteAttributeValueCommand { AttributeNameId = 1, AttributeValueId = 999 };

        // Act & Assert
        await _handler.Handle(command, CancellationToken.None);
    }

    [Fact]
    public async Task ValidRequest_RemovesValue()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var _handler = new RemoveAttributeValueFromAttrName.Handler(_dbContext);

        var attributeName = new AttributeName { ID = 1, Name = "TestAttribute" };
        var attributeValue = new AttributeValue
        {
            ID = 1,
            Value = "TestValue",
            AttributeName = attributeName,
        };
        _dbContext.AttributeNames.Add(attributeName);
        _dbContext.AttributeValues.Add(attributeValue);
        await _dbContext.SaveChangesAsync();

        var command = new RemoveAttributeValueFromAttrName.DeleteAttributeValueCommand { AttributeNameId = 1, AttributeValueId = 1 };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedAttributeValue = await _dbContext.AttributeValues.FindAsync(1);
        Assert.Null(await _dbContext.AttributeValues.FirstOrDefaultAsync());
    }
}
