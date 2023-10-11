using Core;
using Test.Commons;

namespace Order.Core.Test
{
    public class OrderTest
    {
        private static readonly Key _id = new();
        private readonly Order.Order _sut = new(_id);

        [Fact]
        public void AddProduct_WhenValidProductIsAddedToBasket_Success()
        {
            //Arrange
            int quantity = 1;
            SampleProduct product = new() { Quantity = quantity };

            //Act
            _sut.AddProduct(product, quantity);

            //Assert
            Assert.Equivalent(product, _sut.GetProducts().Single());
            Assert.Equal(quantity, _sut.GetProducts().Single().Quantity);
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
            SampleProduct product = new();
            int qty = 3;
            _sut.AddProduct(product, qty);
            double expected = product.Price * qty;

            //Act
            _sut.AddProduct(product, qty);
            var result = _sut.GetTotalAmount();

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AddPayment_WhenCalled_Failure()
        {
            //Arrange
            var payment = new SamplePayment(new Key());

            //Act
            _sut.AddPayment(payment);

            //Assert
            Assert.Null(_sut.GetPayment());
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