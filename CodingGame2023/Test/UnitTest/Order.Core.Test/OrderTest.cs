using Core;
using Moq;
using Order.Core.Interfaces;
using Test.Commons;

namespace Order.Core.Test
{
    public class OrderTest
    {
        private static readonly Key _id = new();
        private Order.Order _sut = new(_id);

        [Fact]
        public void AddProduct_WhenValidProductIsAddedToBasket_Succeeds()
        {
            int quantity = 1;
            SampleProduct product = new() { Quantity = quantity};
            _sut.AddProduct(product, quantity);

            Assert.Equivalent(product, _sut.GetProducts().Single());
            Assert.Equal(quantity, _sut.GetProducts().Single().Quantity);
        }

        [Fact]
        public void AddProduct_WhenNull_Fails()
        {
            _sut.AddProduct(null!, 1);

            Assert.Empty(_sut.GetProducts());
        }

        [Fact]
        public void GetTotalCost_WhenBasketIsEmpty_RetunsZero()
        {
            var result = _sut.GetTotalCost();
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void GetTotalCost_WhenBasketHasProducts_RetunsAmount()
        {
            SampleProduct product = new();
            int qty = 3;
            _sut.AddProduct(product, qty);
            double expected = product.Price * qty;

            var result = _sut.GetTotalCost();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PayWithCard_ShouldThrowNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => _sut.PayWithCard());
        }

        [Fact]
        public void PayWithCash_ShouldThrowNotImplementedException()
        {
            
            Assert.Throws<NotImplementedException>(() => _sut.PayWithCash());
        }
    }
}