namespace Test.ComponentTests.AttributeName;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeName.functions;

public class CreateAttributeNameTests
{
    [Fact]
    public async Task NewAttributeName_ShouldCreateAndReturnDTO()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var handler = new CreateAttributeName.Handler(_dbContext);
        var command = new CreateAttributeName.CreateAttributeNameCommand { Name = "Test Attribute", Description = "Test Description" };

        var expectedDto = new AttributeNameDTO
        {
            Id = 1,
            Name = "Test Attribute",
            Description = "Test Description",
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotStrictEqual(expectedDto, result);
        var createdAttributeName = await _dbContext.AttributeNames.FirstOrDefaultAsync();
        Assert.NotNull(createdAttributeName);
        Assert.Equal(command.Name, createdAttributeName.Name);
        Assert.Equal(command.Description, createdAttributeName.Description);
    }

    [Fact]
    public async Task ExistingAttributeName_ShouldNotCreateDuplicateAndReturnDTO()
    {
        // ARRANGE
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var _dbContext = new DB_Context(dbOption);
        var existingAttributeName = new AttributeName { Name = "Existing Attribute", Description = "Existing Description" };
        await _dbContext.AttributeNames.AddAsync(existingAttributeName);
        await _dbContext.SaveChangesAsync();

        var handler = new CreateAttributeName.Handler(_dbContext);
        var command = new CreateAttributeName.CreateAttributeNameCommand { Name = "Existing Attribute", Description = "Existing Description" };

        var expectedDto = new AttributeNameDTO { Name = "Existing Attribute", Description = "Existing Description" };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotStrictEqual(expectedDto, result);
        Assert.Equal(1, await _dbContext.AttributeNames.CountAsync());
    }
}
