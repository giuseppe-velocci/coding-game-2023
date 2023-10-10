using Core;
using Microsoft.AspNetCore.Http;
using Moq;
using Order.Core.Dto;
using Order.Core.Interfaces;
using Test.Commons;

namespace Order.Api.Test
{
    public class OrderApiServiceAdapterTest
    {
        private readonly Mock<IOrderEndpointsService> _mockService = new();
        private readonly OrderApiServiceAdapter _sut;

        public OrderApiServiceAdapterTest()
        {
            _sut = new(_mockService.Object);
        }

        [Fact]
        public async Task GetOrderAsync_WhenRequestIsValid_ReturnsOkResult()
        {
            // Arrange
            Key orderId = new();
            Core.Order.Order order = new(orderId);
            _mockService
                .Setup(x => x.GetOrderAsync(It.IsAny<Key>()))
                .ReturnsAsync(OperationResult<Core.Order.AbstractOrder>.CreateSuccess(order));

            // Act
            var result = await _sut.GetOrderAsync(orderId);

            // Assert
            Assert.Equivalent(Results.Ok(OperationResult<Core.Order.AbstractOrder>.CreateSuccess(order)), result);
        }

        [Fact]
        public async Task GetOrderAsync_WhenRequestFails_ReturnsBadRequestResult()
        {
            // Arrange
            Key orderId = new();
            string message = "Bad request";
            _mockService
                .Setup(x => x.GetOrderAsync(It.IsAny<Key>()))
                .ReturnsAsync(OperationResult<Core.Order.AbstractOrder>.CreateFailure(message));

            // Act
            var result = await _sut.GetOrderAsync(orderId);

            // Assert
            Assert.Equivalent(Results.BadRequest(OperationResult<Core.Order.AbstractOrder>.CreateFailure(message)), result);
        }

        [Fact]
        public async Task GetDrinksAsync_WhenRequestIsValid_ReturnsOkResult()
        {
            // Arrange
            IEnumerable<IProduct> drinks = new[] { new SampleProduct() };
            _mockService
                .Setup(x => x.GetDrinksAsync())
                .ReturnsAsync(drinks);

            // Act
            var result = await _sut.GetDrinksAsync();

            // Assert
            Assert.Equivalent(Results.Ok(drinks), result);
        }

        [Fact]
        public async Task GetPaymentsAsync_WhenRequestIsValid_ReturnsOkResult()
        {
            // Arrange
            IEnumerable<string> payments = new[] { nameof(SamplePayment) };

            _mockService
                .Setup(x => x.GetPaymentsAsync())
                .ReturnsAsync(payments);

            // Act
            var result = await _sut.GetPaymentsAsync();

            // Assert
            Assert.Equivalent(Results.Ok(payments), result);
        }

        [Fact]
        public async Task CreateOrderAsync_WhenRequestIsValid_ReturnsCreatedResult()
        {
            // Arrange
            var orderId = new Key();
            _mockService
                .Setup(x => x.CreateOrderAsync())
                .ReturnsAsync(OperationResult<Key>.CreateSuccess(orderId));

            // Act
            var result = await _sut.CreateOrderAsync();

            // Assert
            Assert.Equivalent(Results.Created($"order/{orderId.Value}", OperationResult<Key>.CreateSuccess(orderId)), result);
        }

        [Fact]
        public async Task CreateOrderAsync_WhenRequestFails_ReturnsConflictResult()
        {
            // Arrange
            string message = "Conflict";
            _mockService
                .Setup(x => x.CreateOrderAsync())
                .ReturnsAsync(OperationResult<Key>.CreateFailure(message));

            // Act
            var result = await _sut.CreateOrderAsync();

            // Assert
            Assert.Equivalent(Results.Conflict(OperationResult<Key>.CreateFailure(message)), result);
        }

        [Fact]
        public async Task AddPaymentAsync_WhenRequestIsValid_ReturnsCreatedResult()
        {
            // Arrange
            var orderId = new Key();
            var paymentRequest = new PaymentRequest();

            _mockService
                .Setup(x => x.AddPaymentAsync(orderId.Value, paymentRequest))
                .ReturnsAsync(OperationResult<Key>.CreateSuccess(orderId));

            // Act
            var result = await _sut.AddPaymentAsync(orderId.Value, paymentRequest);

            // Assert
            Assert.Equivalent(Results.Created($"order/{orderId.Value}", OperationResult<Key>.CreateSuccess(orderId)), result);
        }

        [Fact]
        public async Task AddPaymentAsync_WhenRequestHasInvalidPayment_ReturnsBadRequestResult()
        {
            // Arrange
            Key orderId = new();
            PaymentRequest paymentRequest = null!;

            // Act
            var result = await _sut.AddPaymentAsync(orderId.Value, paymentRequest);

            // Assert
            Assert.Equivalent(Results.BadRequest(OperationResult<Key>.CreateFailure("Invalid body")), result);
        }

        [Fact]
        public async Task AddToBasketAsync_WhenRequestIsValid_ReturnsCreatedResult()
        {
            // Arrange
            Key orderId = new();
            var productRequest = new ProductRequest { Name = "Coffee", Quantity = 2 };

            _mockService
                .Setup(x => x.AddToBasketAsync(orderId.Value, productRequest))
                .ReturnsAsync(OperationResult<Key>.CreateSuccess(orderId));

            // Act
            var result = await _sut.AddToBasketAsync(orderId.Value, productRequest);

            // Assert
            Assert.Equivalent(Results.Created($"order/{orderId.Value}", OperationResult<Key>.CreateSuccess(orderId)), result);
        }

        [Fact]
        public async Task AddToBasketAsync_WhenRequestHasInvalidProduct_ReturnsBadRequestResult()
        {
            // Arrange
            Key orderId = new();
            ProductRequest productRequest = null!;

            // Act
            var result = await _sut.AddToBasketAsync(orderId.Value, productRequest);

            // Assert
            Assert.Equivalent(Results.BadRequest(OperationResult<Key>.CreateFailure("Invalid body")), result);
        }
    }
}
