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
            int quantity = 1;
            SampleProduct product = new() { Quantity = quantity };
            _sut.AddProduct(product, quantity);

            Assert.Equivalent(product, _sut.GetProducts().Single());
            Assert.Equal(quantity, _sut.GetProducts().Single().Quantity);
        }

        [Fact]
        public void AddProduct_WhenNull_Failure()
        {
            _sut.AddProduct(null!, 1);

            Assert.Empty(_sut.GetProducts());
        }

        [Fact]
        public void GetTotalCost_WhenBasketIsEmpty_RetunsZero()
        {
            var result = _sut.GetTotalAmount();
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetTotalCost_WhenBasketHasProducts_RetunsAmount()
        {
            SampleProduct product = new();
            int qty = 3;
            _sut.AddProduct(product, qty);
            double expected = product.Price * qty;

            var result = _sut.GetTotalAmount();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AddPayment_WhenNoPaymentIsSet_Success()
        {
            var payment = new SamplePayment(new Key());
            _sut.AddPayment(payment);

            Assert.Equal(payment, _sut.GetPayment());
        }

        [Fact]
        public void AddPayment_WhenPaymentIsSet_OverridesAndSuccess()
        {
            _sut.AddPayment(new SamplePayment(new Key()));
            var payment = new SamplePayment(new Key());
            _sut.AddPayment(payment);

            Assert.Equal(payment, _sut.GetPayment());
        }

        [Fact]
        public void RemoveProduct_WhenExistsingProductIsRemovedToBasket_Success()
        {
            SampleProduct product = new();
            _sut.AddProduct(product, 1);
            _sut.RemoveProduct(product);

            Assert.Empty(_sut.GetProducts());
        }
        
        [Fact]
        public void RemoveProduct_WhenNotExistsingProductIsRemovedToBasket_Success()
        {
            SampleProduct product = new();
            _sut.RemoveProduct(product);

            Assert.Empty(_sut.GetProducts());
        }
    }
}