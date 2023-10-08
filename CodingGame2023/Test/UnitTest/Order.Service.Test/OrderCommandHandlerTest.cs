using Core;
using Moq;
using Order.Core.Interfaces;
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
        public void Handle_WhenCreateOrderCommandAndAggregateApplySucceeds_Success()
        {
            //Arrange
            _mockAggregate.Setup(x => x.Apply(It.IsAny<IEvent>())).Returns(OperationResult<Key>.CreateSuccess(new Key()));

            //Act
            var result = _sut.Handle(new CreateOrderCommand());
            Assert.True(result.Success);
        }

        [Fact]
        public void Handle_WhenCreateOrderCommandAndAggregateApplyFails_Failure()
        {
            //Arrange
            _mockAggregate.Setup(x => x.Apply(It.IsAny<IEvent>())).Returns(OperationResult<Key>.CreateFailure(""));

            //Act
            var result = _sut.Handle(new CreateOrderCommand());

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void Handle_WhenAddProductToBasketCommandAndAggregateApplyFails_Failure()
        {
            //Arrange
            _mockProductStore.Setup(x => x.Find(It.IsAny<string>())).Returns(OperationResult<IProduct>.CreateSuccess(new SampleProduct()));
            _mockAggregate.Setup(x => x.Apply(It.IsAny<IEvent>())).Returns(OperationResult<Key>.CreateFailure(""));

            //Act
            var result = _sut.Handle(new AddProductToBasketCommand(new Key(), "prod", 1));

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void Handle_WhenAddProductToBasketCommandAndQuantityLowerThanOne_Failure()
        {
            //Arrange
            _mockProductStore.Setup(x => x.Find(It.IsAny<string>())).Returns(OperationResult<IProduct>.CreateSuccess(new SampleProduct()));

            //Act
            var result = _sut.Handle(new AddProductToBasketCommand(new Key(), "prod", 0));

            //Assert
            Assert.False(result.Success);
        }
        
        [Fact]
        public void Handle_WhenAddProductToBasketCommandAndProductIsNotFound_Failure()
        {
            //Arrange
            _mockProductStore.Setup(x => x.Find(It.IsAny<string>())).Returns(OperationResult<IProduct>.CreateFailure(""));
            
            //Act
            var result = _sut.Handle(new AddProductToBasketCommand(new Key(), "prod", 0));

            //Assert
            Assert.False(result.Success);
        }
        
        [Fact]
        public void Handle_WhenAddPaymentCommandAndAggregateApplyFails_Fails()
        {
            //Arrange
            _mockPaymentStore.Setup(x => x.Find(It.IsAny<string>())).Returns(OperationResult<IPayment>.CreateSuccess(new SamplePayment(new Key())));
            _mockAggregate.Setup(x => x.Apply(It.IsAny<IEvent>())).Returns(OperationResult<Key>.CreateFailure(""));

            //Act
            var result = _sut.Handle(new AddPaymentCommand(new Key(), "payment"));

            //Assert
            Assert.False(result.Success);
        }
        
        [Fact]
        public void Handle_WhenAddPaymentCommandAndPaymentFound_Success()
        {
            //Arrange
            _mockPaymentStore.Setup(x => x.Find(It.IsAny<string>())).Returns(OperationResult<IPayment>.CreateSuccess(new SamplePayment(new Key())));
            _mockAggregate.Setup(x => x.Apply(It.IsAny<IEvent>())).Returns(OperationResult<Key>.CreateSuccess(new Key()));

            //Act
            var result = _sut.Handle(new AddPaymentCommand(new Key(), "payment"));

            //Assert
            Assert.True(result.Success);
        }
        
        [Fact]
        public void Handle_WhenAddPaymentCommandAndPaymentIsNotFound_Failure()
        {
            //Arrange
            _mockPaymentStore.Setup(x => x.Find(It.IsAny<string>())).Returns(OperationResult<IPayment>.CreateFailure(""));

            //Act
            var result = _sut.Handle(new AddPaymentCommand(new Key(), "payment"));

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void Handle_WhenCommandIsUnhandled_Failure()
        {
            //Act
            var result = _sut.Handle(new UnhandledCommand());

            //Assert
            Assert.False(result.Success);
        }
    }
}