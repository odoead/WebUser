namespace Test.ComponentTests.Product;

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Test.mocks;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.Product.Exceptions;
using WebUser.PricingService.DTO;
using WebUser.shared.RepoWrapper;
using static WebUser.features.Product.Functions.GetProductPage;

public class GetProductPageTests
{
    private Mock<IServiceWrapper> serviceWrapper;

    public GetProductPageTests()
    {
        serviceWrapper = ServiceWrapperMock.CreateMock();
    }

    [Fact]
    public async Task ReturnProductPageDTO_WhenProductExists()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var category1 = new Category
        {
            ID = 1,
            Name = "Category 1",
            ParentCategory = null,
        };
        var category2 = new Category
        {
            ID = 2,
            Name = "Category 2",
            ParentCategory = category1,
        };
        dbcontext.Categories.AddRange(category1, category2);
        var product = new Product
        {
            ID = 1,
            Name = "Test Product",
            Description = "Test Description",
            Price = 100,
            Stock = 10,
            ReservedStock = 2,
            AttributeValues = new List<ProductAttributeValue>
            {
                new ProductAttributeValue
                {
                    AttributeValue = new AttributeValue
                    {
                        ID = 1,
                        Value = "Red",
                        AttributeName = new AttributeName
                        {
                            ID = 1,
                            Name = "Color",
                            Categories = new List<AttributeNameCategory> { new AttributeNameCategory { Category = category2 } },
                        },
                    },
                },
            },
            Discounts = new List<Discount>
            {
                new Discount { ID = 1, DiscountVal = 10 },
            },
        };
        dbcontext.Products.Add(product);
        await dbcontext.SaveChangesAsync();

        // Mocking services
        serviceWrapper
            .Setup(s => s.Pricing.ApplyDiscountAsync(It.IsAny<List<int>>()))
            .ReturnsAsync(
                new List<DiscountRecordDTO>
                {
                    new DiscountRecordDTO
                    {
                        ProductId = product.ID,
                        Type = DiscountType.ProductDiscount,
                        ValueTypes = new List<DiscountValueType> { DiscountValueType.Absolute },
                        AbsoluteDiscountValue = 10,
                    },
                }
            );
        serviceWrapper.Setup(s => s.Product.CalculatePriceWithCumulativeDiscounts(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<List<DiscountRecordDTO>>())).Returns(90); // Assuming a discount application of 90
        serviceWrapper.Setup(s => s.Category.GetParentCategoriesLine(It.IsAny<int>())).ReturnsAsync(new List<Category> { category1 });

        var handler = new Handler(dbcontext, serviceWrapper.Object);

        // Act
        var result = await handler.Handle(new GetProductPageByIDQuery { Id = 1 }, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.ID, result.ID);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(90, result.AfterDiscountPrice);
        Assert.Equal(8, result.Stock);
        Assert.Equal(2, result.AttributeValues.Count);
    }

    [Fact]
    public async Task ThrowProductNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var context = new DB_Context(dbOption);

        var handler = new Handler(context, serviceWrapper.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ProductNotFoundException>(async () => await handler.Handle(new GetProductPageByIDQuery { Id = 999 }, CancellationToken.None));
    }
}
