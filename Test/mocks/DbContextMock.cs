namespace Test.mocks;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebUser.Data;
using WebUser.Domain.entities;

public class DbContextMock
{
    public static DB_Context GetMock(List<Category> categories, List<Order> orders, List<Cart> carts)
    {
        var categoryDbSetMock = CreateMockDbSet(categories);
        var orderDbSetMock = CreateMockDbSet(orders);
        var cartDbSetMock = CreateMockDbSet(carts);

        // Create and configure Mock
        var dbContextMock = new Mock<DB_Context>();
        dbContextMock.Setup(m => m.Categories).Returns(categoryDbSetMock.Object);
        dbContextMock.Setup(m => m.Orders).Returns(orderDbSetMock.Object);
        dbContextMock.Setup(m => m.Carts).Returns(cartDbSetMock.Object);

        return dbContextMock.Object;
    }

    private static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data)
        where T : class
    {
        var queryable = data.AsQueryable();
        var dbSetMock = new Mock<DbSet<T>>();
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        dbSetMock.Setup(x => x.Add(It.IsAny<T>())).Callback<T>(data.Add);
        dbSetMock.Setup(x => x.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(data.AddRange);
        dbSetMock.Setup(x => x.Remove(It.IsAny<T>())).Callback<T>(entity => data.Remove(entity));
        dbSetMock
            .Setup(x => x.RemoveRange(It.IsAny<IEnumerable<T>>()))
            .Callback<IEnumerable<T>>(entities =>
            {
                foreach (var t in entities)
                {
                    data.Remove(t);
                }
            });
        return dbSetMock;
    }
}
