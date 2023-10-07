namespace Order.Api.Test
{
    public class OrderEndpointsTest
    {
        private readonly OrderEndpoints _sut;

        public OrderEndpointsTest()
        {
            _sut = new OrderEndpoints();
        }

        [Fact]
        public void CreateOrder_WhenCalled_ReturnsKey()
        {
            var result = _sut.CreateOrder();
            Assert.NotNull(result?.Value);
        }
    }
}