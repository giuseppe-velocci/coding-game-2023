using Core;
using Moq;
using Order.Core.Interfaces;
using Order.Service.Aggregates;
using Order.Service.Events;
using Test.Commons;

namespace Order.Service.Test
{
    public class OrderAggregateTests
    {
        private readonly OrderAggregate _sut;
        private readonly Mock<IEventStore<IOrder>> _mockEventStore = new();

        public OrderAggregateTests()
        {
            _sut = new(_mockEventStore.Object);
        }

        [Fact]
        public void Apply_WhenOrderCreatedEvent_Success()
        {
            // Arrange
            var orderCreatedEvent = new OrderCreatedEvent();
            _mockEventStore.Setup(x => x.GetEvents(orderCreatedEvent.Id)).Returns(Array.Empty<IEvent>());

            // Act
            var result = _sut.Apply(orderCreatedEvent);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(orderCreatedEvent.Id, _sut.GetInstance(orderCreatedEvent.Id)!.Id);
            _mockEventStore.Verify(store => store.Store(orderCreatedEvent), Times.Once);
        }

        [Fact]
        public void Apply_WhenOrderCreatedEventAndEventAlreadyExists_Failure()
        {
            // Arrange
            var orderCreatedEvent = new OrderCreatedEvent();
            _mockEventStore.Setup(x => x.GetEvents(orderCreatedEvent.Id)).Returns(new[] { orderCreatedEvent });

            // Act
            var result = _sut.Apply(orderCreatedEvent);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(orderCreatedEvent.Id, _sut.GetInstance(orderCreatedEvent.Id)!.Id);
            _mockEventStore.Verify(store => store.Store(orderCreatedEvent), Times.Never);
        }

        [Fact]
        public void Apply_WhenProductAddedToBasketEvent_Success()
        {
            // Arrange
            var orderCreatedEvent = new OrderCreatedEvent();
            _mockEventStore.Setup(x => x.GetEvents(It.IsAny<Key>())).Returns(new[] { orderCreatedEvent });

            var productAddedEvent = new ProductAddedToBasketEvent(
                orderCreatedEvent.Id,
                new SampleProduct(),
                2
            );

            // Act
            var result = _sut.Apply(productAddedEvent);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, _sut.GetInstance(orderCreatedEvent.Id)!.GetProducts().Single().Quantity);
            _mockEventStore.Verify(store => store.Store(productAddedEvent), Times.Once);
        }

        [Fact]
        public void Apply_WhenUnhandledEvent_Failure()
        {
            // Arrange
            var invalidEvent = new GenericEvent(0);
            _mockEventStore.Setup(x => x.GetEvents(It.IsAny<Key>())).Returns(Array.Empty<IEvent>());


            // Act
            var result = _sut.Apply(invalidEvent);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Invalid event", result.Message);
        }
    }
}
