/*namespace Test.mocks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Moq;

internal class FirstMock
{
    public static Mock<Cart> GetMock()
    {
        var mockContext = new Mock<db_context>();
        var mockSet = new Mock<DbSet<Cart>>();

        // Setup the mock set to return a queryable list of PrivateMessages
        var privateMessages = new List<Cart>
        {
            new Cart
            {
                Id = 1,
                Subject = "Test",
                Description = "Test Description",
                CreatedAt = DateTime.Now
            },
            // ... other test data
        }.AsQueryable();

        mockSet.As<IQueryable<Cart>>().Setup(m => m.Provider).Returns(privateMessages.Provider);
        mockSet.As<IQueryable<Cart>>().Setup(m => m.Expression).Returns(privateMessages.Expression);
        mockSet.As<IQueryable<Cart>>().Setup(m => m.ElementType).Returns(privateMessages.ElementType);
        mockSet.As<IQueryable<Cart>>().Setup(m => m.GetEnumerator()).Returns(privateMessages.GetEnumerator());

        // Setup the mock context to return the mock set
        mockContext.Setup(c => c.PrivateMessages).Returns(mockSet.Object);
    }
}
*/
