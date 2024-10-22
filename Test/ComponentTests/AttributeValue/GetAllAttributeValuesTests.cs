namespace Test.ComponentTests.AttributeValue;

using System;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.functions;
using WebUser.features.AttributeValue;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.AttributeValue.functions;
using WebUser.shared.RequestForming.features;

public class GetAllAttributeValuesTests
{
    private readonly DB_Context _dbContext;
    private readonly AddAttributeValueToAttrName.Handler _handler;

    public GetAllAttributeValuesTests()
    {
        var options = new DbContextOptionsBuilder<DB_Context>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        _dbContext = new DB_Context(options);
        _handler = new AddAttributeValueToAttrName.Handler(_dbContext);
    }

    [Fact]
    public async Task ShouldReturnPaginatedList()
    {
        // Arrange
        var attributeValues = new List<AttributeValue>
        {
            new()
            {
                ID = 1,
                Value = "TestValue 1",
                AttributeNameID = 1,
            },
            new()
            {
                ID = 2,
                Value = "TestValue 2",
                AttributeNameID = 2,
            },
            new()
            {
                ID = 3,
                Value = "TestValue 3",
                AttributeNameID = 3,
            },
            new()
            {
                ID = 4,
                Value = "TestValue 4",
                AttributeNameID = 4,
            },
        };
        await _dbContext.AttributeValues.AddRangeAsync(attributeValues);
        await _dbContext.SaveChangesAsync();

        var handler = new GetAllAttrValues.Handler(_dbContext);
        var query = new GetAllAttrValues.GetAllAttrValueQuery(new AttributeValuesRequestParameters { PageNumber = 1, PageSize = 2 });

        var dtos = new List<AttributeValueDTO>
        {
            new() { ID = 1, Value = "TestValue 1" },
            new() { ID = 2, Value = "TestValue 2" },
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsType<PagedList<AttributeValueDTO>>(result);
        Assert.True(result.PagesStat.IsFirst);
        Assert.False(result.PagesStat.IsLast);
        Assert.True(result.PagesStat.HasNext);
        Assert.False(result.PagesStat.HasPrev);
        Assert.False(result.PagesStat.HasPrev);
        Assert.Equal(4, result.PagesStat.TotalCount);
        Assert.Equal(2, result.PagesStat.PageCount);
        Assert.Equal(2, result.PagesStat.PageSize);
        Assert.Equal(2, result.Count);
    }
}
