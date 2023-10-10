using Core;
using Moq;
using Order.Core.Dto;
using Order.Core.Interfaces;
using Order.Service.Commands;
using Test.Commons;

namespace Order.Service.Test
{
    public class OrderEndpointsTest
    {
        private readonly OrderEndpointsService _sut;
        private readonly Mock<ICommandHandler<IOrder>> _mockHandler = new();
        private readonly Mock<IProductStore> _mockProductStore = new();
        private readonly Mock<IOrderStore> _mockOrderStore = new();
        private readonly Mock<IPaymentStore> _mockPaymentStore = new();

        public OrderEndpointsTest()
        {
            _sut = new OrderEndpointsService(
                _mockHandler.Object,
                _mockProductStore.Object,
                _mockOrderStore.Object,
                _mockPaymentStore.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_WhenCalled_ReturnsKey()
        {
            //Arrange
            _mockHandler
                .Setup(x => x.HandleAsync(It.IsAny<CreateOrderCommand>()))
                .ReturnsAsync(OperationResult<Key>.CreateSuccess(new()));

            //Act
            var result = await _sut.CreateOrderAsync();

            //Asert
            Assert.NotNull(result?.Value);
        }

        [Fact]
        public async Task AddToBasketAsync_WhenCalled_ReturnsKey()
        {
            //Arrange
            Key id = new();
            _mockHandler
                .Setup(x => x.HandleAsync(It.IsAny<AddProductToBasketCommand>()))
                .ReturnsAsync(OperationResult<Key>.CreateSuccess(new()));

            //Act
            var result = await _sut.AddToBasketAsync(id.Value, new ProductRequest());

            //Asert
            Assert.NotNull(result?.Value);
        }

        [Fact]
        public async Task AddPaymentAsync_WhenCalled_ReturnsKey()
        {
            //Arrange
            Key id = new();
            _mockHandler
                .Setup(x => x.HandleAsync(It.IsAny<AddPaymentCommand>()))
                .ReturnsAsync(OperationResult<Key>.CreateSuccess(new()));

            //Act
            var result = await _sut.AddPaymentAsync(id.Value, new PaymentRequest());

            //Asert
            Assert.NotNull(result?.Value);
        }

        [Fact]
        public async Task GetDrinksAsync_WhenCalled_ReturnsKey()
        {
            //Arrange
            IProduct[] products = new IProduct[] { new SampleProduct() };
            _mockProductStore
                .Setup(x => x.GetProductsAsync())
                .ReturnsAsync(products);

            //Act
            var result = await _sut.GetDrinksAsync();

            //Asert
            Assert.Equal(products, result);
        }

        [Fact]
        public async Task GetPaymentsAsync_WhenCalled_ReturnsKey()
        {
            //Arrange
            string[] payments = new string[] { nameof(SamplePayment) };
            _mockPaymentStore
                .Setup(x => x.GetPaymentsAsync())
                .ReturnsAsync(payments);

            //Act
            var result = await _sut.GetPaymentsAsync();

            //Asert
            Assert.Equal(payments, result);
        }
    }
}