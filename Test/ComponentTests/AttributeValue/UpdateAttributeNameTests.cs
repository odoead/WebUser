namespace Test.ComponentTests.AttributeValue;

using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeValue.Exceptions;
using WebUser.features.AttributeValue.functions;

public class UpdateAttributeValueTests
{


    private readonly DB_Context _dbContext;
    private readonly UpdateAttributeValue.Handler _handler;

    public UpdateAttributeValueTests()
    {

        var options = new DbContextOptionsBuilder<DB_Context>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        _dbContext = new DB_Context(options);
        _handler = new UpdateAttributeValue.Handler(_dbContext);
    }

    [Fact]
    public async Task ExistingAttributeValue_UpdatesValue()
    {
        // Arrange
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
        // Arrange
        var command = new UpdateAttributeValue.UpdateAttributeValueCommand(999, "New Value");

        // Act & Assert
        await Assert.ThrowsAsync<AttributeValueNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
