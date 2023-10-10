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
        public async Task Apply_WhenOrderCreatedEvent_Success()
        {
            // Arrange
            var orderCreatedEvent = new OrderCreatedEvent();
            _mockEventStore.Setup(x => x.GetEventsAsync(orderCreatedEvent.Id)).ReturnsAsync(Array.Empty<IEvent>());

            // Act
            var result = await _sut.Apply(orderCreatedEvent);
            var instance = await _sut.GetInstance(orderCreatedEvent.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(orderCreatedEvent.Id, instance!.Id);
            _mockEventStore.Verify(store => store.StoreAsync(orderCreatedEvent), Times.Once);
        }

        [Fact]
        public async Task Apply_WhenOrderCreatedEventAndEventAlreadyExists_Failure()
        {
            // Arrange
            var orderCreatedEvent = new OrderCreatedEvent();
            _mockEventStore.Setup(x => x.GetEventsAsync(orderCreatedEvent.Id)).ReturnsAsync(new[] { orderCreatedEvent });

            // Act
            var result = await _sut.Apply(orderCreatedEvent);
            var instance = await _sut.GetInstance(orderCreatedEvent.Id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(orderCreatedEvent.Id, instance!.Id);
            _mockEventStore.Verify(store => store.StoreAsync(It.IsAny<IEvent>()), Times.Never);
        }

        [Fact]
        public async Task Apply_WhenProductAddedToBasketEvent_Success()
        {
            // Arrange
            var orderCreatedEvent = new OrderCreatedEvent();
            _mockEventStore.Setup(x => x.GetEventsAsync(It.IsAny<Key>())).ReturnsAsync(new[] { orderCreatedEvent });

            var productAddedEvent = new ProductAddedToBasketEvent(
                orderCreatedEvent.Id,
                new SampleProduct(),
                2
            );

            // Act
            var result = await _sut.Apply(productAddedEvent);
            var instance = await _sut.GetInstance(orderCreatedEvent.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, instance!.GetProducts().Single().Quantity);
            _mockEventStore.Verify(store => store.StoreAsync(It.Is<IEvent>(x => x.Version == 1)), Times.Once);
        }

        [Fact]
        public async Task Apply_WhenProductAddedToBasketEventAndProductIsAlreadyThere_OverwritesQuantity()
        {
            // Arrange
            var orderCreatedEvent = new OrderCreatedEvent();
            var firstProductAddedEvent = new ProductAddedToBasketEvent(
                orderCreatedEvent.Id,
                new SampleProduct(),
                1
            );
            _mockEventStore
                .Setup(x => x.GetEventsAsync(It.IsAny<Key>()))
                .ReturnsAsync(new IEvent[] {
                    orderCreatedEvent,
                    firstProductAddedEvent
                });

            var productAddedEvent = new ProductAddedToBasketEvent(
                orderCreatedEvent.Id,
                new SampleProduct(),
                2
            );

            // Act
            var result = await _sut.Apply(productAddedEvent);
            var instance = await _sut.GetInstance(orderCreatedEvent.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, instance!.GetProducts().Single().Quantity);
            _mockEventStore.Verify(store => store.StoreAsync(It.Is<IEvent>(x => x.Version == 2)), Times.Once);
        }

        [Fact]
        public async Task Apply_WhenProductAddedToBasketAndEventDoesNotExists_Failure()
        {
            // Arrange
            var id = new Key();
            _mockEventStore.Setup(x => x.GetEventsAsync(It.IsAny<Key>())).ReturnsAsync(Array.Empty<IEvent>());

            var productAddedEvent = new ProductAddedToBasketEvent(
                id,
                new SampleProduct(),
                2
            );

            // Act
            var result = await _sut.Apply(productAddedEvent);
            var instance = await _sut.GetInstance(id);

            // Assert
            Assert.False(result.Success);
            _mockEventStore.Verify(store => store.StoreAsync(It.IsAny<IEvent>()), Times.Never);
        }
        
        [Fact]
        public async Task Apply_WhenProductAddedToBasketAndOrderIsClosed_Failure()
        {
            // Arrange
            var orderCreatedEvent = new OrderCreatedEvent();
            Key id = orderCreatedEvent.Id;
            var paymentAddedEvent = new PaymentAddedEvent(orderCreatedEvent.Id, new SamplePayment(id));
            _mockEventStore.Setup(x => x.GetEventsAsync(It.IsAny<Key>())).ReturnsAsync(new IEvent[] { orderCreatedEvent, paymentAddedEvent });

            var productAddedEvent = new ProductAddedToBasketEvent(
                id,
                new SampleProduct(),
                2
            );

            // Act
            var result = await _sut.Apply(productAddedEvent);
            var instance = await _sut.GetInstance(id);

            // Assert
            Assert.False(result.Success);
            _mockEventStore.Verify(store => store.StoreAsync(It.IsAny<IEvent>()), Times.Never);
        }

        [Fact]
        public async Task Apply_WhenPaymentAddedEventAndOrderHasProducts_Success()
        {
            // Arrange
            var orderCreatedEvent = new OrderCreatedEvent();
            var productAddedEvent = new ProductAddedToBasketEvent(orderCreatedEvent.Id, new SampleProduct(), 2);
            _mockEventStore.Setup(x => x.GetEventsAsync(It.IsAny<Key>())).ReturnsAsync(new IEvent[] { orderCreatedEvent, productAddedEvent });
            var paymentAddedEvent = new PaymentAddedEvent(orderCreatedEvent.Id, new SamplePayment(orderCreatedEvent.Id));

            // Act
            var result = await _sut.Apply(paymentAddedEvent);
            var instance = await _sut.GetInstance(orderCreatedEvent.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(paymentAddedEvent.Id, instance!.Id);
            _mockEventStore.Verify(store => store.StoreAsync(It.Is<IEvent>(x => x.Version == 2)), Times.Once);
        }
        
        [Fact]
        public async Task Apply_WhenPaymentAddedEventAndOrderHasNowProducts_Failure()
        {
            // Arrange
            var orderCreatedEvent = new OrderCreatedEvent();
            _mockEventStore.Setup(x => x.GetEventsAsync(It.IsAny<Key>())).ReturnsAsync(new[] { orderCreatedEvent });
            var paymentAddedEvent = new PaymentAddedEvent(orderCreatedEvent.Id, new SamplePayment(orderCreatedEvent.Id));

            // Act
            var result = await _sut.Apply(paymentAddedEvent);
            var instance = await _sut.GetInstance(orderCreatedEvent.Id);

            // Assert
            Assert.False(result.Success);
            _mockEventStore.Verify(store => store.StoreAsync(It.Is<IEvent>(x => x.Version == 1)), Times.Never);
        }

        [Fact]
        public async Task Apply_WhenPaymentAddedEventAndOrderDoesNotExists_Failure()
        {
            // Arrange
            Key id = new();
            var paymentAddedEvent = new PaymentAddedEvent(id, new SamplePayment(id));
            _mockEventStore.Setup(x => x.GetEventsAsync(paymentAddedEvent.Id)).ReturnsAsync(Array.Empty<IEvent>());

            // Act
            var result = await _sut.Apply(paymentAddedEvent);

            // Assert
            Assert.False(result.Success);
            _mockEventStore.Verify(store => store.StoreAsync(It.IsAny<IEvent>()), Times.Never);
        }

        [Fact]
        public async Task Apply_WhenUnhandledEvent_Failure()
        {
            // Arrange
            var invalidEvent = new SampleEvent(0);
            _mockEventStore.Setup(x => x.GetEventsAsync(It.IsAny<Key>())).ReturnsAsync(Array.Empty<IEvent>());


            // Act
            var result = await _sut.Apply(invalidEvent);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Invalid event", result.Message);
        }
    }
}
