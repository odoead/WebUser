namespace Test.ComponentTests.AttributeValue;

using System.Threading.Tasks;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeValue.Exceptions;
using WebUser.features.AttributeValue.functions;

public class UpdateAttributeValueTests
{
    [Fact]
    public async Task ExistingAttributeValue_UpdatesValue()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var _handler = new UpdateAttributeValue.Handler(_dbContext);
        var attributeValue = new AttributeValue { ID = 1, Value = "Old Value" };
        _dbContext.AttributeValues.Add(attributeValue);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateAttributeValue.UpdateAttributeValueCommand(1, "New Value");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedAttributeValue = await _dbContext.AttributeValues.FindAsync(1);
        Assert.Equal("New Value", updatedAttributeValue.Value);
    }

    [Fact]
    public async Task AttributeValueNonExists_ThrowsAttributeValueNotFoundException()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var _handler = new UpdateAttributeValue.Handler(_dbContext);
        var command = new UpdateAttributeValue.UpdateAttributeValueCommand(999, "New Value");

        // Act & Assert
        await Assert.ThrowsAsync<AttributeValueNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
