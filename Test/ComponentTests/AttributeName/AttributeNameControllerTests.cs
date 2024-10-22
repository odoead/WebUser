using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebUser.features.AttributeName;
using WebUser.features.AttributeName.functions;

// Your namespace here
public class AttributeNameControllerTests
{
    private readonly Mock<IMediator> mediatorMock;
    private readonly AttributeNameController controller;

    public AttributeNameControllerTests()
    {
        mediatorMock = new Mock<IMediator>();
        controller = new AttributeNameController(mediatorMock.Object);
    }

    [Fact]
    public async Task AddValue_ShouldReturnNoContent_WhenCommandIsSuccessful()
    {
        // ARRANGE
        int attributeNameId = 1;
        string attributeValue = "TestValue";

        mediatorMock
            .Setup(m => m.Send(It.IsAny<AddAttributeValueToAttrName.AddAttributeValueCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // ACT
        var result = await controller.AddValue(attributeNameId, attributeValue);

        // ASSERT
        var noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, noContentResult.StatusCode);

        mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<AddAttributeValueToAttrName.AddAttributeValueCommand>(cmd =>
                        cmd.AttributeNameID == attributeNameId && cmd.AttributeValue == attributeValue
                    ),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
