using Order.Core.Interfaces;

namespace Test.Commons
{
    public class SampleProduct : IProduct
    {
        public string Name => nameof(SampleProduct);
        public double Price => 10.25;
        public int Quantity { get; init; }

        public IProduct UpdateQuantity(int quantity)
        {
            return new SampleProduct() { Quantity = quantity };
        }
    }
}
