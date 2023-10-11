using Core;
using Order.Core.Order;
using Test.Commons;

namespace Order.Core.Test
{
    public class ClosedOrderTest
    {
        private static readonly Key _id = new();
        private static readonly Order.Order order = new(_id);
        private readonly Order.ClosedOrder _sut = new(order);

        [Fact]
        public void AddProduct_WhenValidProductIsAddedToBasket_Failure()
        {
            //Arrange
            int quantity = 1;
            SampleProduct product = new() { Quantity = quantity };

            //Act
            _sut.AddProduct(product, quantity);

            //Assert
            Assert.Empty(_sut.GetProducts());
        }

        [Fact]
        public void AddProduct_WhenNull_Failure()
        {
            //Act
            _sut.AddProduct(null!, 1);

            //Assert
            Assert.Empty(_sut.GetProducts());
        }

        [Fact]
        public void GetTotalCost_WhenBasketIsEmpty_RetunsZero()
        {
            //Act
            var result = _sut.GetTotalAmount();

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetTotalCost_WhenBasketHasProducts_RetunsAmount()
        {
            //Arrange
            Order.Order orderWithProducts = new(_id);
            SampleProduct product = new();
            int qty = 3;
            orderWithProducts.AddProduct(product, qty);
            ClosedOrder sut = new(orderWithProducts);

            double expected = product.Price * qty;

            //Act
            var result = sut.GetTotalAmount();

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AddPayment_WhenNoPaymentIsSet_Success()
        {
            //Arrange
            var payment = new SamplePayment(new Key());

            //Act
            _sut.AddPayment(payment);

            //Assert
            Assert.Equal(payment, _sut.GetPayment());
        }

        [Fact]
        public void AddPayment_WhenPaymentIsSet_OverridesAndSuccess()
        {
            //Arrange
            var payment = new SamplePayment(new Key());

            //Act
            _sut.AddPayment(new SamplePayment(new Key()));
            _sut.AddPayment(payment);

            //Assert
            Assert.Equal(payment, _sut.GetPayment());
        }

        [Fact]
        public void RemoveProduct_WhenExistsingProductIsRemovedToBasket_Success()
        {
            //Arrange
            SampleProduct product = new();

            //Act
            _sut.AddProduct(product, 1);
            _sut.RemoveProduct(product);

            //Assert
            Assert.Empty(_sut.GetProducts());
        }

        [Fact]
        public void RemoveProduct_WhenNotExistsingProductIsRemovedToBasket_Success()
        {
            //Arrange
            SampleProduct product = new();

            //Act
            _sut.RemoveProduct(product);

            //Assert
            Assert.Empty(_sut.GetProducts());
        }
    }
}