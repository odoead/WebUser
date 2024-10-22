namespace Test.ComponentTests.AttributeName;

using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeName.functions;

public class CreateAttributeNameTests
{


    private readonly DB_Context _dbContext;
    private readonly AddAttributeValueToAttrName.Handler _handler;

    public CreateAttributeNameTests()
    {

        var options = new DbContextOptionsBuilder<DB_Context>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        _dbContext = new DB_Context(options);
        _handler = new AddAttributeValueToAttrName.Handler(_dbContext);
    }

    [Fact]
    public async Task NewAttributeName_ShouldCreateAndReturnDTO()
    {
        // Arrange
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
        // Arrange
        var existingAttributeName = new AttributeName { Name = "Existing Attribute", Description = "Existing Description" };
        await _dbContext.AttributeNames.AddAsync(existingAttributeName);
        await _dbContext.SaveChangesAsync();

        var handler = new CreateAttributeName.Handler(_dbContext);
        var command = new CreateAttributeName.CreateAttributeNameCommand { Name = "Existing Attribute", Description = "Existing Description" };

        var expectedDto = new AttributeNameDTO
        {
            Name = "Existing Attribute",
            Description = "Existing Description",
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotStrictEqual(expectedDto, result);
        Assert.Equal(1, await _dbContext.AttributeNames.CountAsync());
    }
}
