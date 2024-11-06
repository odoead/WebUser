namespace Test.ComponentTests.Promotion;

using System;
using System.Collections.Generic;
using WebUser.Domain.entities;
using WebUser.features.Promotion.PromoBuilder.Actions;
using WebUser.features.Promotion.PromoBuilder.Condition;
using WebUser.PricingService.DTO;

public class PromotionBuilderTests
{
    [Fact]
    public void GetDiscountShouldApplyAbsoluteDiscount()
    {
        // ARRANGE
        var products = new List<Product>
        {
            new Product { ID = 1, Price = 100 },
            new Product { ID = 2, Price = 200 },
        };
        double discountValue = 20;
        int discountPercent = 0;

        // ACT
        var result = products.GetDiscountAct(discountValue, discountPercent);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        foreach (var discountRecord in result)
        {
            Assert.Equal(DiscountType.PromotionDiscount, discountRecord.Type);
            Assert.Contains(DiscountValueType.Absolute, discountRecord.ValueTypes);
            Assert.Equal(discountValue, discountRecord.AbsoluteDiscountValue);
            Assert.Null(discountRecord.PercentDiscountValue);
        }
    }

    [Fact]
    public void GetDiscountShouldApplyPercentageDiscount()
    {
        // ARRANGE
        var products = new List<Product>
        {
            new Product { ID = 1, Price = 100 },
            new Product { ID = 2, Price = 200 },
        };
        double discountValue = 0;
        int discountPercent = 10;

        // ACT
        var result = products.GetDiscountAct(discountValue, discountPercent);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        foreach (var discountRecord in result)
        {
            Assert.Equal(DiscountType.PromotionDiscount, discountRecord.Type);
            Assert.Contains(DiscountValueType.Percentage, discountRecord.ValueTypes);
            Assert.Equal(discountPercent, discountRecord.Percent);
            Assert.Equal(
                Math.Floor(discountRecord.ProductId == 1 ? 100 * (double)discountPercent / 100 : 200 * discountPercent / 100),
                discountRecord.PercentDiscountValue
            );
            Assert.Null(discountRecord.AbsoluteDiscountValue);
        }
    }

    [Fact]
    public void GetDiscountShouldApplyBothDiscounts()
    {
        // ARRANGE
        var products = new List<Product>
        {
            new Product { ID = 1, Price = 100 },
            new Product { ID = 2, Price = 200 },
        };
        double discountValue = 15;
        int discountPercent = 10;

        // ACT
        var result = products.GetDiscountAct(discountValue, discountPercent);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        foreach (var discountRecord in result)
        {
            Assert.Equal(DiscountType.PromotionDiscount, discountRecord.Type);
            Assert.Contains(DiscountValueType.Absolute, discountRecord.ValueTypes);
            Assert.Contains(DiscountValueType.Percentage, discountRecord.ValueTypes);
            Assert.Equal(discountValue, discountRecord.AbsoluteDiscountValue);
            var basePrice = discountRecord.ProductId == 1 ? 100 : 200;
            Assert.Equal(Math.Floor(basePrice * (double)discountPercent / 100), discountRecord.PercentDiscountValue);
            Assert.Equal(discountPercent, discountRecord.Percent);
        }
    }

    [Fact]
    public void GetDiscountForItemtAct_ShouldApplyPromotionDiscount()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { ID = 1, Price = 100 },
            new Product { ID = 2, Price = 200 },
        };

        var promoProducts = new List<Product>
        {
            new Product { ID = 1, Price = 100 },
        };
        double discountValue = 10;
        int discountPercent = 10;

        // Act
        var result = products.GetDiscountForItemtAct(promoProducts, discountValue, discountPercent);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var discountRecord = result.First();
        Assert.Equal(1, discountRecord.ProductId);
        Assert.Equal(10, discountRecord.AbsoluteDiscountValue);
        Assert.Equal(10, discountRecord.Percent);
    }

    [Fact]
    public void GetFreeItemAct_ShouldGenerateFreeItems()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product
            {
                ID = 1,
                Name = "Product 1",
                Price = 100,
            },
            new Product
            {
                ID = 2,
                Name = "Product 2",
                Price = 200,
            },
        };

        var freeProducts = new List<Product>
        {
            new Product
            {
                ID = 1,
                Name = "Product 1",
                Price = 100,
            },
        };
        int quantity = 2;

        // Act
        var result = products.GetFreeItemAct(freeProducts, quantity);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var freeItem = result.First();
        Assert.Equal(1, freeItem.ProductId);
        Assert.Equal(0, freeItem.FinalSinglePrice);
        Assert.Equal(2, freeItem.Quantity);
        Assert.Equal(100, freeItem.AppliedDiscounts.First().PercentDiscountValue);
    }

    [Fact]
    public void GetPointsAct_ShouldGeneratePoints()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { ID = 1, Price = 100 },
            new Product { ID = 2, Price = 200 },
        };

        int pointsValue = 50;
        int pointsPercent = 10;
        int expireDays = 30;

        // Act
        var result = products.GetPointsAct(pointsValue, pointsPercent, expireDays);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(80, result.BalanceLeft);
        Assert.True(result.IsExpirable);
        Assert.NotNull(result.ExpireDate);
        Assert.Equal(DateTime.UtcNow.AddDays(expireDays).Date, result.ExpireDate.Date);
    }

    private Product CreateProductWithAttributes(int productId, params int[] attributeValueIds)
    {
        return new Product
        {
            ID = productId,
            AttributeValues = attributeValueIds.Select(id => new ProductAttributeValue { AttributeValue = new AttributeValue { ID = id } }).ToList(),
        };
    }

    // Test for AttributeCondition
    [Fact]
    public void AttributeCondition_ShouldReturnTrue_WhenProductsHaveAttributes()
    {
        // Arrange
        var attribute1 = new AttributeValue { ID = 1 };
        var attribute2 = new AttributeValue { ID = 2 };
        var product = new Product
        {
            ID = 1,
            AttributeValues = new List<ProductAttributeValue>
            {
                new ProductAttributeValue { AttributeValue = attribute1 },
                new ProductAttributeValue { AttributeValue = attribute2 },
            },
        };
        var products = new List<Product> { product };

        // Act
        bool result = products.HasAttributes(new List<AttributeValue> { attribute1, attribute2 });

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AttributeCondition_ShouldReturnFalse_WhenProductsDoNotHaveAttributes()
    {
        // Arrange
        var attribute1 = new AttributeValue { ID = 1 };
        var attribute2 = new AttributeValue { ID = 2 };
        var attribute3 = new AttributeValue { ID = 3 };
        var product = new Product
        {
            ID = 1,
            AttributeValues = new List<ProductAttributeValue>
            {
                new ProductAttributeValue { AttributeValue = attribute1 },
                new ProductAttributeValue { AttributeValue = attribute2 },
            },
        };
        var products = new List<Product> { product };

        // Act
        bool result = products.HasAttributes(new List<AttributeValue> { attribute3 });

        // Assert
        Assert.False(result);
    }

    // Test for CategoryCondition
    [Fact]
    public void CategoryCondition_ShouldReturnTrue_WhenProductsHaveCategories()
    {
        // Arrange
        var category = new Category { ID = 1 };
        var attributeName = new AttributeName { Categories = new List<AttributeNameCategory> { new AttributeNameCategory { Category = category } } };
        var attributeValue = new AttributeValue { AttributeName = attributeName };
        var product = new Product { AttributeValues = new List<ProductAttributeValue> { new ProductAttributeValue { AttributeValue = attributeValue } } };
        var products = new List<Product> { product };

        // Act
        bool result = products.HasCategories(new List<Category> { category });

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CategoryCondition_ShouldReturnFalse_WhenProductsDoNotHaveCategories()
    {
        // Arrange
        var category = new Category { ID = 2 };
        var attributeName = new AttributeName { Categories = new List<AttributeNameCategory> { new AttributeNameCategory { Category = new Category { ID = 1 } } } };
        var attributeValue = new AttributeValue { AttributeName = attributeName };
        var product = new Product { AttributeValues = new List<ProductAttributeValue> { new ProductAttributeValue { AttributeValue = attributeValue } } };
        var products = new List<Product> { product };

        // Act
        bool result = products.HasCategories(new List<Category> { category });

        // Assert
        Assert.False(result);
    }

    // Test for FirstOrderCondition
    [Fact]
    public void FirstOrderCondition_ShouldReturnTrue_WhenUserHasNoOrders()
    {
        // Arrange
        var user = new User { Orders = new List<Order>() };

        // Act
        bool result = user.IsFirstOrder();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void FirstOrderCondition_ShouldReturnFalse_WhenUserHasOrders()
    {
        // Arrange
        var user = new User { Orders = new List<Order> { new Order() } };

        // Act
        bool result = user.IsFirstOrder();

        // Assert
        Assert.False(result);
    }

    // Test for SpendCondition
    [Fact]
    public void SpendCondition_ShouldReturnTrue_WhenCartExceedsRequiredTotal()
    {
        // Arrange
        var productCalculation = new ProductCalculationDTO
        {
            BasePrice = 100,
            Quantity = 2,
            AppliedDiscounts = new List<DiscountRecordDTO>
            {
                new DiscountRecordDTO
                {
                    Type = DiscountType.ProductDiscount,
                    PercentDiscountValue = 10,
                    Percent = 10,
                    ValueTypes = new List<DiscountValueType> { DiscountValueType.Percentage },
                },
                new DiscountRecordDTO
                {
                    Type = DiscountType.ProductDiscount,
                    PercentDiscountValue = 20,
                    Percent = 20,
                    ValueTypes = new List<DiscountValueType> { DiscountValueType.Percentage },
                },
            },
        };
        var cart = new List<ProductCalculationDTO> { productCalculation };

        // Act
        bool result = cart.IsPriceBiggerThan(100);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void SpendCondition_ShouldReturnFalse_WhenCartDoesNotExceedRequiredTotal()
    {
        // Arrange
        var productCalculation = new ProductCalculationDTO
        {
            BasePrice = 100,
            Quantity = 1,
            AppliedDiscounts = new List<DiscountRecordDTO>
            {
                new DiscountRecordDTO
                {
                    Type = DiscountType.ProductDiscount,
                    PercentDiscountValue = 99,
                    Percent = 99,
                    ValueTypes = new List<DiscountValueType> { DiscountValueType.Percentage },
                },
            },
        };
        var cart = new List<ProductCalculationDTO> { productCalculation };

        // Act
        bool result = cart.IsPriceBiggerThan(100);

        // Assert
        Assert.False(result);
    }

    // Test for QuantityCondition
    [Fact]
    public void QuantityCondition_ShouldReturnTrue_WhenProductsMeetQuantityCondition()
    {
        // Arrange
        var product = new Product { ID = 1 };
        var productsWithQuantities = new List<(Product Product, int Quantity)> { (product, 3) };

        // Act
        bool result = productsWithQuantities.HasQuantityProducts(new List<Product> { product }, 2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void QuantityCondition_ShouldReturnFalse_WhenProductsDoNotMeetQuantityCondition()
    {
        // Arrange
        var product = new Product { ID = 1 };
        var productsWithQuantities = new List<(Product Product, int Quantity)> { (product, 1) };

        // Act
        bool result = productsWithQuantities.HasQuantityProducts(new List<Product> { product }, 2);

        // Assert
        Assert.False(result);
    }
}
