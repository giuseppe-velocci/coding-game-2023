using Order.Core.Interfaces;

namespace Order.Core.Drinks
{
    public class Tea : IProduct
    {
        public string Name { get; } = nameof(Tea);
        public double Price { get; } = 3.00;
        public int Quantity { get; init; }

        public IProduct UpdateQuantity(int quantity)
        {
            return new Tea() { Quantity = quantity };
        }
    }
}
