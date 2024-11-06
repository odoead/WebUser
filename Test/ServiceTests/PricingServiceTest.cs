namespace Test.ServiceTests;

using System.Collections.Generic;
using System.Threading.Tasks;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.PricingService;
using WebUser.PricingService.DTO;

public class PricingServiceTest
{
    [Fact]
    public async Task ApplyDiscountAsync_ActiveDiscounts_ReturnsDiscountRecordDTO()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);
        var product = new Product
        {
            ID = 1,
            Price = 100,
            Description = "Test",
            Name = "Test name",
            Discounts = new List<Discount>
            {
                new Discount
                {
                    ID = 1,
                    DiscountPercent = 10,
                    ActiveFrom = DateTime.UtcNow.AddDays(-5),
                    ActiveTo = DateTime.UtcNow.AddDays(5),
                },
                new Discount
                {
                    ID = 2,
                    DiscountVal = 5,
                    ActiveFrom = DateTime.UtcNow.AddDays(-5),
                    ActiveTo = DateTime.UtcNow.AddDays(5),
                },
            },
        };
        dbcontext.Products.Add(product);
        await dbcontext.SaveChangesAsync();

        // Act
        var result = await service.ApplyDiscountAsync(new List<int> { 1 });
        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.PercentDiscountValue == 10);
        Assert.Contains(result, r => r.AbsoluteDiscountValue == 5);
    }

    [Fact]
    public async Task ApplyCouponsAsync_ValidCoupon_ReturnsActivatedCoupon()
    {
        //Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);
        var product = new Product
        {
            ID = 1,
            Price = 200,
            Coupons = new List<Coupon>(),
            Description = "Test",
            Name = "Test name",
        };
        var coupon = new Coupon
        {
            ID = 1,
            ProductID = 1,
            Code = "SUMMER10",
            DiscountPercent = 10,
            IsActivated = false,
            ActiveFrom = DateTime.UtcNow.AddDays(-5),
            ActiveTo = DateTime.UtcNow.AddDays(5),
            CreatedAt = DateTime.UtcNow.AddDays(-7),
        };
        dbcontext.Products.Add(product);
        dbcontext.Coupons.Add(coupon);
        await dbcontext.SaveChangesAsync();

        // Act
        var result = await service.ApplyCouponsAsync(new List<int> { product.ID }, "SUMMER10");

        // Assert
        Assert.Single(result);
        Assert.Equal(20, result.First().record.PercentDiscountValue);
        Assert.Equal(10, result.First().record.Percent);
        Assert.True(result.First().coupon.IsActivated);
    }

    [Fact]
    public async Task GenerateOrderAsync_ApplyDiscountsAndCoupons_SuccessfulOrder()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);
        var user = new User { FirstName = "Test", Id = "user1", LastName = "LastTest" };
        var product = new Product
        {
            ID = 1,
            Price = 100,
            Name = "Test Product",
            Description = "Test",
        };
        var coupon = new Coupon
        {
            Code = "DISCOUNT10",
            DiscountVal = 10,
            ProductID = 1,
            ActiveFrom = DateTime.UtcNow.AddDays(-5),
            ActiveTo = DateTime.UtcNow.AddDays(5),
            CreatedAt = DateTime.UtcNow.AddDays(-7),
        };
        dbcontext.Users.Add(user);
        dbcontext.Products.Add(product);
        dbcontext.Coupons.Add(coupon);
        await dbcontext.SaveChangesAsync();

        var orderProducts = new List<(int productId, int quantity)> { (1, 1) };

        // Act
        var (order, newPoint, activatedPoints, activatedCoupons) = await service.GenerateOrderAsync(orderProducts, user, "DISCOUNT10", 0);

        // Assert
        Assert.Equal(90, order.OrderCost);
        Assert.Single(order.Products);
        Assert.Single(order.Products.Select(q => q.AppliedDiscounts));
        Assert.Single(activatedCoupons);
        Assert.Equal("DISCOUNT10", activatedCoupons.First().Code);
    }




    [Fact]
    public async Task ApplyPointsAsync_UserHasEnoughPoints_ReturnsCorrectDiscount()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);

        // Seed data - user with enough points
        var user = new User { Email = "Test@gmail.com", FirstName = "Test", Id = "user1", LastName = "LastTest" };
        var point1 = new Point { ID = 1, UserID = user.Id, Value = 100, BalanceLeft = 100, IsUsed = false };
        var point2 = new Point { ID = 2, UserID = user.Id, Value = 50, BalanceLeft = 50, IsUsed = false };
        dbcontext.Users.Add(user);
        dbcontext.Points.AddRange(point1, point2);
        await dbcontext.SaveChangesAsync();

        // Act
        var result = await service.ApplyPointsAsync(user.Id, 120);

        // Assert
        Assert.Equal(120, result.discount.AbsoluteDiscountValue); // Total discount applied is 120
        Assert.Equal(2, result.activatedPoints.Count); // Two points are activated
        Assert.Equal(0, point1.BalanceLeft); //fully used
        Assert.Equal(30, point2.BalanceLeft); // remain balance
        Assert.All(result.activatedPoints, p => Assert.True(p.IsUsed));
    }

    [Fact]
    public async Task ApplyPointsAsync_UserHasInsufficientPoints_ReturnsZeroDiscount()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);

        // Seed data - user with insufficient points
        var user = new User { Email = "Test@gmail.com", FirstName = "Test", Id = "user1", LastName = "LastTest" };
        var point1 = new Point { ID = 1, UserID = user.Id, Value = 50, BalanceLeft = 50, IsUsed = false };
        dbcontext.Users.Add(user);
        dbcontext.Points.Add(point1);
        await dbcontext.SaveChangesAsync();

        // Act
        var result = await service.ApplyPointsAsync(user.Id, 100);

        // Assert
        Assert.Equal(0, result.discount.AbsoluteDiscountValue); // Total discount applied is 0
        Assert.Empty(result.activatedPoints); // No points activated
        Assert.False(point1.IsUsed); // Point remains unused
        Assert.Equal(50, point1.BalanceLeft); // Point balance unchanged
    }

    [Fact]
    public async Task ApplyPointsAsync_UserHasExactPoints_ReturnsExactDiscount()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);

        // Seed data - user with exact points
        var user = new User { Email = "Test@gmail.com", FirstName = "Test", Id = "user1", LastName = "LastTest" };
        var point1 = new Point { ID = 1, UserID = user.Id, Value = 100, BalanceLeft = 100, IsUsed = false };
        dbcontext.Users.Add(user);
        dbcontext.Points.Add(point1);
        await dbcontext.SaveChangesAsync();

        // Act
        var result = await service.ApplyPointsAsync(user.Id, 100);

        // Assert
        Assert.Equal(100, result.discount.AbsoluteDiscountValue); // Total discount applied is 100
        Assert.Single(result.activatedPoints); // Only one point is activated
        Assert.True(result.activatedPoints.First().IsUsed);
        Assert.Equal(0, point1.BalanceLeft); // Point fully used
    }




    [Fact]
    public async Task ApplyPromosAsync_ValidPercentageDiscountPromotion_AppliesDiscount()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);

        var user = new User { FirstName = "Test", Id = "user1", LastName = "LastTest" };
        var product = new Product
        {
            ID = 1,
            Price = 100,
            Description = "Test",
            Name = "Test name",
        };
        var promotion = new Promotion
        {
            DiscountPercent = 10,
            ConditionProducts = new List<PromotionProduct> { new PromotionProduct { Product = product } },
            ActiveFrom = DateTime.UtcNow.AddDays(-5),
            ActiveTo = DateTime.UtcNow.AddDays(5),
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            Description = "Test",
            Name = "Test name",
            IsFirstOrder = false
        };
        dbcontext.Users.Add(user);
        dbcontext.Products.Add(product);
        dbcontext.Promotions.Add(promotion);
        await dbcontext.SaveChangesAsync();

        var productCalculations = new List<ProductCalculationDTO> { new ProductCalculationDTO { ProductId = 1, Quantity = 1 } };

        // Act
        var result = await service.ApplyPromosAsync(productCalculations, user);

        // Assert
        Assert.Single(result.records);
        Assert.Equal(10, result.records.First().PercentDiscountValue); // 10% of 100 is 10
    }

    [Fact]
    public async Task ApplyPromosAsync_ValidFixedDiscountPromotion_AppliesDiscount()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);

        var user = new User { FirstName = "Test", Id = "user2", LastName = "LastTest" };
        var product = new Product
        {
            ID = 2,
            Price = 150,
            Description = "Test",
            Name = "Test name",
        };
        var promotion = new Promotion
        {
            DiscountVal = 20,
            ActionProducts = new List<PromotionPromProduct> { new PromotionPromProduct { Product = product } },
            ActiveFrom = DateTime.UtcNow.AddDays(-5),
            ActiveTo = DateTime.UtcNow.AddDays(5),
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            Description = "Test",
            Name = "Test name",
        };

        dbcontext.Users.Add(user);
        dbcontext.Products.Add(product);
        dbcontext.Promotions.Add(promotion);
        await dbcontext.SaveChangesAsync();

        var productCalculations = new List<ProductCalculationDTO> { new ProductCalculationDTO { ProductId = 2, Quantity = 1 } };

        // Act
        var result = await service.ApplyPromosAsync(productCalculations, user);

        // Assert
        Assert.Single(result.records);
        Assert.Equal(20, result.records.First().AbsoluteDiscountValue);
    }





    [Fact]
    public async Task GenerateOrderAsync_NoDiscountsOrCoupons_FullPrice()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);
        var user = new User { FirstName = "Test", Id = "user1", LastName = "LastTest" };
        var product = new Product
        {
            ID = 1,
            Price = 100,
            Name = "Basic Product",
            Description = "Basic",
        };

        dbcontext.Users.Add(user);
        dbcontext.Products.Add(product);
        await dbcontext.SaveChangesAsync();

        var orderProducts = new List<(int productId, int quantity)> { (1, 1) };

        // Act
        var (order, newPoint, activatedPoints, activatedCoupons) = await service.GenerateOrderAsync(orderProducts, user, "", 0);

        // Assert
        Assert.Equal(100, order.OrderCost);
        Assert.Single(order.Products);
        Assert.Empty(activatedCoupons);
        Assert.Empty(activatedPoints);
        Assert.Null(newPoint);
    }

    [Fact]
    public async Task GenerateOrderAsync_ApplyPromotions_AddsFreeItem()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);
        var user = new User { FirstName = "Test", Id = "user2", LastName = "LastTest" };
        var product = new Product
        {
            ID = 1,
            Price = 150,
            Name = "Test Product",
            Description = "Test",
        };
        var promo = new Promotion
        {
            DiscountVal = 20,
            GetQuantity = 1,
            ActiveFrom = DateTime.UtcNow.AddDays(-5),
            ActiveTo = DateTime.UtcNow.AddDays(5),
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            Name = "Test Product",
            Description = "Test",
            ActionProducts = new List<PromotionPromProduct> { new PromotionPromProduct { Product = product, } },

        };
        dbcontext.Users.Add(user);
        dbcontext.Products.Add(product);
        dbcontext.Promotions.Add(promo);
        await dbcontext.SaveChangesAsync();

        var orderProducts = new List<(int productId, int quantity)> { (1, 1) };

        // Act
        var (order, newPoint, activatedPoints, activatedCoupons) = await service.GenerateOrderAsync(orderProducts, user, "", 0);

        // Assert
        Assert.Equal(130, order.OrderCost);
        Assert.Equal(2, order.Products.Count);
        Assert.Equal(2, order.Products.Where(p => p.ProductId == 1).Count());
        Assert.Equal(1, order.Products.Where(p => p.FinalSinglePrice == 0).Count());

    }

    [Fact]
    public async Task GenerateOrderAsync_AllDiscountsAndCoupons_AppliesAll()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);
        var user = new User { FirstName = "Test", Id = "user2", LastName = "LastTest" };
        var product = new Product
        {
            ID = 1,
            Price = 200,
            Name = "Test Product",
            Description = "Test",
        };
        var promo = new Promotion
        {
            DiscountPercent = 10,
            ActiveFrom = DateTime.UtcNow.AddDays(-5),
            ActiveTo = DateTime.UtcNow.AddDays(5),
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            Name = "Test Product",
            Description = "Test",
        };
        var points = new Point { UserID = "user2", Value = 15, BalanceLeft = 15, };

        dbcontext.Users.Add(user);
        dbcontext.Products.Add(product);
        dbcontext.Promotions.Add(promo);
        dbcontext.Points.Add(points);
        await dbcontext.SaveChangesAsync();

        var orderProducts = new List<(int productId, int quantity)> { (1, 1) };

        // Act
        var (order, newPoint, activatedPoints, activatedCoupons) = await service.GenerateOrderAsync(orderProducts, user, "VIP", 10);

        // Assert
        Assert.Equal(170, order.OrderCost);
        Assert.Empty(activatedCoupons);
        Assert.Single(order.Products);
        Assert.Single(activatedPoints);
        Assert.Equal(new List<int> { 5 }, activatedPoints.Select(q => q.BalanceLeft).ToList());
        Assert.Equal(10, order.UsedBonusPoints);
    }
    [Fact]
    public async Task GenerateOrderAsync_MultipleProductsWithDiscountsAndCoupons_AppliesCorrectDiscounts()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);
        var user = new User { FirstName = "Test", Id = "user4", LastName = "LastTest" };
        var product1 = new Product { ID = 1, Price = 150, Name = "Product 1", Description = "Test" };
        var product2 = new Product { ID = 2, Price = 100, Name = "Product 2", Description = "Test" };
        var coupon = new Coupon
        {
            Code = "DISCOUNT20",
            DiscountVal = 20,
            ProductID = 1,
            ActiveFrom = DateTime.UtcNow.AddDays(-5),
            ActiveTo = DateTime.UtcNow.AddDays(5),
            CreatedAt = DateTime.UtcNow.AddDays(-7),
        };
        var promo = new Promotion
        {
            DiscountPercent = 10,
            ConditionProducts = new List<PromotionProduct> { new PromotionProduct { Product = product2 } },
            ActionProducts = new List<PromotionPromProduct> { new PromotionPromProduct { Product = product2 } },
            ActiveFrom = DateTime.UtcNow.AddDays(-5),
            ActiveTo = DateTime.UtcNow.AddDays(5),
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            Name = "Test Product",
            Description = "Test",
        };

        dbcontext.Users.Add(user);
        dbcontext.Products.AddRange(product1, product2);
        dbcontext.Coupons.Add(coupon);
        dbcontext.Promotions.Add(promo);
        await dbcontext.SaveChangesAsync();

        var orderProducts = new List<(int productId, int quantity)> { (1, 1), (2, 1) };

        // Act
        var (order, newPoint, activatedPoints, activatedCoupons) = await service.GenerateOrderAsync(orderProducts, user, "DISCOUNT20", 0);

        // Assert
        Assert.Equal(220, order.OrderCost);
        Assert.Equal(2, order.Products.Count);
        Assert.Single(activatedCoupons);
        Assert.Equal("DISCOUNT20", activatedCoupons.First().Code);
        Assert.Empty(activatedPoints);
    }


    [Fact]
    public async Task GenerateOrderAsync_AllDiscounts_Coupon_Promo_Points_AppliesAllCorrectly()
    {
        // Arrange
        var dbOption = InmemoryTestDBGenerator.CreateDbContextOptions();
        var dbcontext = new DB_Context(dbOption);
        var service = new PricingService(dbcontext);
        var user = new User { FirstName = "Test", Id = "user5", LastName = "LastTest" };

        // Adding product
        var product1 = new Product
        {
            ID = 1,
            Price = 200,
            Name = "Product 1",
            Description = "Test",
            Discounts = new List<Discount>
            {
                new Discount
                {
                    ID = 1,
                    DiscountVal = 15,
                    CreatedAt= DateTime.UtcNow.AddDays(-7),
                    ActiveFrom = DateTime.UtcNow.AddDays(-5),
                    ActiveTo = DateTime.UtcNow.AddDays(5),
                }, }
        };
        var product2 = new Product { ID = 2, Price = 100, Name = "Product 2", Description = "Test" };

        // Adding a coupon for product1
        var coupon = new Coupon
        {
            Code = "SAVE20",
            DiscountVal = 20,
            ProductID = 1,
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            ActiveFrom = DateTime.UtcNow.AddDays(-5),
            ActiveTo = DateTime.UtcNow.AddDays(5),
        };

        // Adding promotion for product2 with a 10% discount
        var promo = new Promotion
        {
            DiscountPercent = 10,
            ActionProducts = new List<PromotionPromProduct> { new PromotionPromProduct { Product = product2 } },
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            ActiveFrom = DateTime.UtcNow.AddDays(-5),
            ActiveTo = DateTime.UtcNow.AddDays(5),
            Name = "Test Product",
            Description = "Test",
        };

        // Adding user points
        var points = new Point { UserID = "user5", Value = 100, BalanceLeft = 100, };

        dbcontext.Users.Add(user);
        dbcontext.Products.AddRange(product1, product2);
        dbcontext.Coupons.Add(coupon);
        dbcontext.Promotions.Add(promo);
        dbcontext.Points.Add(points);
        await dbcontext.SaveChangesAsync();

        // Specifying order products
        var orderProducts = new List<(int productId, int quantity)> { (1, 1), (2, 1) };

        // Act
        var (order, newPoint, activatedPoints, activatedCoupons) = await service.GenerateOrderAsync(orderProducts, user, "SAVE20", 10);

        // Assert
        // 1:200-15-20)= 165
        // 2:100*0.9= 90
        // 165+90= 255
        // 10 points: 255-10=245
        Assert.Equal(245, order.OrderCost);
        Assert.Equal(2, order.Products.Count);

        // Product 1 checks
        var product1Calculation = order.Products.First(p => p.ProductId == 1);
        Assert.Equal(165, product1Calculation.FinalSinglePrice);
        Assert.Equal(2, product1Calculation.AppliedDiscounts.Count); // Discount, coupon

        // Product 2 checks
        var product2Calculation = order.Products.First(p => p.ProductId == 2);
        Assert.Equal(90, product2Calculation.FinalSinglePrice);
        Assert.Equal(1, product2Calculation.AppliedDiscounts.Count); // Promotion 

        Assert.Single(activatedCoupons);
        Assert.Equal("SAVE20", activatedCoupons.First().Code);
        Assert.Single(activatedPoints);
        Assert.Equal(10, order.UsedBonusPoints);
    }

}
