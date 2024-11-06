namespace Test.ComponentTests.AttributeValue;

using System.Threading.Tasks;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.AttributeValue.Exceptions;
using WebUser.features.AttributeValue.functions;

public class GetByIDAttributeValueTests
{
    [Fact]
    public async Task ExistingId_ReturnsAttributeValueDTO()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var _handler = new GetByIDAttributeValue.Handler(_dbContext);
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
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var _handler = new GetByIDAttributeValue.Handler(_dbContext);
        var query = new GetByIDAttributeValue.GetByIDAttrValueQuery { Id = 999 };

        // Act & Assert
        await Assert.ThrowsAsync<AttributeValueNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
