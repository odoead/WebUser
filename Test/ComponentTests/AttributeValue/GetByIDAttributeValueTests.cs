namespace Test.ComponentTests.AttributeValue;

using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.AttributeValue.Exceptions;
using WebUser.features.AttributeValue.functions;

public class GetByIDAttributeValueTests
{

    private readonly DB_Context _dbContext;
    private readonly GetByIDAttributeValue.Handler _handler;

    public GetByIDAttributeValueTests()
    {

        var options = new DbContextOptionsBuilder<DB_Context>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        _dbContext = new DB_Context(options);
        _handler = new GetByIDAttributeValue.Handler(_dbContext);
    }

    [Fact]
    public async Task ExistingId_ReturnsAttributeValueDTO()
    {
        // Arrange
        var attributeValue = new AttributeValue
        {
            ID = 1,
            Value = "TestValue",
            AttributeNameID = 1,
        };
        _dbContext.AttributeValues.Add(attributeValue);
        await _dbContext.SaveChangesAsync();

        var expectedDto = new AttributeValueDTO { ID = 1, Value = "TestValue" };

        var query = new GetByIDAttributeValue.GetByIDAttrValueQuery { Id = 1 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.ID, result.ID);
        Assert.Equal(expectedDto.Value, result.Value);
    }

    [Fact]
    public async Task NonExistingId_ThrowsAttributeValueNotFoundException()
    {
        // Arrange
        var query = new GetByIDAttributeValue.GetByIDAttrValueQuery { Id = 999 };

        // Act & Assert
        await Assert.ThrowsAsync<AttributeValueNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }


}
