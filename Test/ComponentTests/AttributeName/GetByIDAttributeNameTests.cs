namespace Test.ComponentTests.AttributeName;

using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeName.Exceptions;
using WebUser.features.AttributeName.functions;

public class GetByIDAttributeValueTests
{

    private readonly DB_Context _dbContext;
    private readonly GetByIDAttributeName.Handler _handler;

    public GetByIDAttributeValueTests()
    {

        var options = new DbContextOptionsBuilder<DB_Context>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        _dbContext = new DB_Context(options);
        _handler = new GetByIDAttributeName.Handler(_dbContext);
    }

    [Fact]
    public async Task ExistingId_ReturnsAttributeNameDTO()
    {
        // Arrange
        var attributeName = new AttributeName { ID = 1, Name = "TestAttribute" };
        _dbContext.AttributeNames.Add(attributeName);
        await _dbContext.SaveChangesAsync();

        var expectedDto = new AttributeNameDTO { Id = 1, Name = "TestAttribute" };

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
        // Arrange
        var query = new GetByIDAttributeName.GetByIDAttrNameQuery { Id = 999 };

        // Act & Assert
        await Assert.ThrowsAsync<AttributeNameNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }


}
