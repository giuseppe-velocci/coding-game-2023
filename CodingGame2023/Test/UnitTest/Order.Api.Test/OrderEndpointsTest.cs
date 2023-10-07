using Core;
using Moq;
using Order.Core.Interfaces;
using Order.Service.Commands;

namespace Order.Api.Test
{
    public class OrderEndpointsTest
    {
        private readonly OrderEndpoints _sut;
        private readonly Mock<ICommandHandler<IOrder>> _mockHandler;

        public OrderEndpointsTest()
        {
            _mockHandler = new();
            _sut = new OrderEndpoints(_mockHandler.Object);
        }

        [Fact]
        public void CreateOrder_WhenCalled_ReturnsKey()
        {
            _mockHandler
                .Setup(x => x.Handle(It.IsAny<CreateOrderCommand>()))
                .Returns(OperationResult<Key>.CreateSuccess(new()));
            var result = _sut.CreateOrder();
            Assert.NotNull(result?.Value);
        }
    }
}