using Core;
using Moq;
using Order.Core.Interfaces;
using Order.Service.Aggregates;
using Order.Service.CommandHandlers;
using Order.Service.Commands;
using Order.Service.Events;
using Test.Commons;

namespace Order.Service.Test
{
    public class OrderCommandHandlerTest
    {
        private readonly OrderCommandHandler _sut;
        private readonly Mock<IAggregate<IOrder>> _mockAggregate = new();
        private readonly Mock<IProductStore> _mockProductStore = new();
        private readonly Mock<IPaymentStore> _mockPaymentStore = new();

        public OrderCommandHandlerTest()
        {
            _sut = new OrderCommandHandler(_mockAggregate.Object, _mockProductStore.Object, _mockPaymentStore.Object);
        }

        [Fact]
        public void Handle_WhenCommandIsUnhandled_Failure()
        {
            var result = _sut.Handle(new UnhandledCommand());
            Assert.False(result.Success);
        }

        [Fact]
        public void Handle_WhenCreateOrderCommandAndAggregateApplySucceeds_Success()
        {
            _mockAggregate.Setup(x => x.Apply(It.IsAny<OrderCreatedEvent>())).Returns(OperationResult<Key>.CreateSuccess(new Key()));
            var result = _sut.Handle(new CreateOrderCommand());
            Assert.True(result.Success);
        }

        [Fact]
        public void Handle_WhenCreateOrderCommandAndAggregateApplyFails_Failure()
        {
            _mockAggregate.Setup(x => x.Apply(It.IsAny<OrderCreatedEvent>())).Returns(OperationResult<Key>.CreateFailure(""));
            var result = _sut.Handle(new CreateOrderCommand());
            Assert.False(result.Success);
        }
    }
}