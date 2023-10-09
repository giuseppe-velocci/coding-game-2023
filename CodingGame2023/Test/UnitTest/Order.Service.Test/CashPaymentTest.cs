using Core;
using Order.Core.Interfaces;
using Order.Core.Payments;

namespace Order.Service.Test
{
    public class CashPaymentTest
    {
        private readonly CashPayment _sut = new();

        [Theory]
        [InlineData(0)]
        [InlineData(1.0)]
        [InlineData(9.99)]
        [InlineData(10.0)]
        public void IsAllowed_WhenAmountLowerOrEqualThanThreshold_Success(double amount)
        {
            // Act
            var result = _sut.IsAllowed(amount);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(10.01)]
        [InlineData(10.1)]
        [InlineData(1000.0)]
        public void IsAllowed_WhenAmountGreaterThanThreshold_Failure(double amount)
        {
            // Act
            var result = _sut.IsAllowed(amount);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UpdateOrderId_WhenInvoked_ReturnsNewInstance()
        {
            //Act
            Key id = new();
            var result = _sut.UpdateOrderId(id);

            //Assert
            Assert.Equal(id, result.OrderId);
            Assert.NotEqual(_sut.OrderId, result.OrderId);
            Assert.NotEqual(result, _sut);
        }

        [Fact]
        public void PaymentType_WhenRead_IsCash()
        {
            //Act
            var result = _sut.PaymentType;

            //Assert
            Assert.Equal(PaymentType.Cash, result);
        }   
    }
}
