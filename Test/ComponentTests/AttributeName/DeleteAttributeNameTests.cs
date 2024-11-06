namespace Test.ComponentTests.AttributeName;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.Exceptions;
using WebUser.features.AttributeName.functions;

public class DeleteAttributeNameTests
{
    [Fact]
    public async Task ExistingAttributeName_ShouldDelete()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var attributeName = new AttributeName
        {
            ID = 1,
            Name = "Test Attribute",
            Description = "Test Description",
        };
        await _dbContext.AttributeNames.AddAsync(attributeName);
        await _dbContext.SaveChangesAsync();

        var handler = new DeleteAttributeName.Handler(_dbContext);
        var command = new DeleteAttributeName.DeleteAttributeNameCommand { ID = 1 };

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Empty(await _dbContext.AttributeNames.ToListAsync());
    }

    [Fact]
    public async Task NonExistingAttributeName_ShouldThrowException()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var handler = new DeleteAttributeName.Handler(_dbContext);
        var command = new DeleteAttributeName.DeleteAttributeNameCommand { ID = 999 };

        // Act & Assert
        await Assert.ThrowsAsync<AttributeNameNotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}
