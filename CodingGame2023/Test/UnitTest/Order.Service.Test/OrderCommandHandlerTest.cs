using Core;
using Moq;
using Order.Service.Aggregates;
using Order.Service.Commands;
using Order.Service.Events;
using Test.Commons;

namespace Order.Service.Test
{
    public class OrderCommandHandlerTest
    {
        private readonly OrderCommandHandler _sut;
        private readonly Mock<IOrderAggregate> _mockAggregate;

        public OrderCommandHandlerTest()
        {
            _mockAggregate = new Mock<IOrderAggregate>();
            _sut = new OrderCommandHandler(_mockAggregate.Object);
        }

        [Fact]
        public void Handle_WhenCommandIsUnhandled_Fails()
        {
            var result = _sut.Handle(new UnhandledCommand());
            Assert.False(result.Success);
        }

        [Fact]
        public void Handle_WhenCreateOrderCommandAndAggregateApplySucceeds_Succeeds()
        {
            _mockAggregate.Setup(x => x.Apply(It.IsAny<OrderCreatedEvent>())).Returns(OperationResult<Key>.CreateSuccess(new Key()));
            var result = _sut.Handle(new CreateOrderCommand());
            Assert.True(result.Success);
        }

        [Fact]
        public void Handle_WhenCreateOrderCommandAndAggregateApplyFails_Fails()
        {
            _mockAggregate.Setup(x => x.Apply(It.IsAny<OrderCreatedEvent>())).Returns(OperationResult<Key>.CreateFailure(""));
            var result = _sut.Handle(new CreateOrderCommand());
            Assert.False(result.Success);
        }
    }
}