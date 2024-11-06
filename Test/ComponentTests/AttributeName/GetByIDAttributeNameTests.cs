namespace Test.ComponentTests.AttributeName;

using System.Threading.Tasks;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeName.Exceptions;
using WebUser.features.AttributeName.functions;

public class GetByIDAttributeValueTests
{
    [Fact]
    public async Task ExistingId_ReturnsAttributeNameDTO()
    {
        // ARRANGE

        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var attributeName = new AttributeName { ID = 1, Name = "TestAttribute" };
        _dbContext.AttributeNames.Add(attributeName);
        await _dbContext.SaveChangesAsync();

        var expectedDto = new AttributeNameDTO { Id = 1, Name = "TestAttribute" };
        var _handler = new GetByIDAttributeName.Handler(_dbContext);
        var query = new GetByIDAttributeName.GetByIDAttrNameQuery { Id = 1 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.Name, result.Name);
    }

    [Fact]
    public async Task NonExistingId_ThrowsAttributeNameNotFoundException()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var query = new GetByIDAttributeName.GetByIDAttrNameQuery { Id = 999 };
        var _handler = new GetByIDAttributeName.Handler(_dbContext);

        // Act & Assert
        await Assert.ThrowsAsync<AttributeNameNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
