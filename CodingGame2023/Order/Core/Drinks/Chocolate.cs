using Order.Core.Interfaces;

namespace Order.Core.Drinks
{
    public class Chocolate : IProduct
    {
        public string Name { get; } = nameof(Chocolate);
        public double Price { get; } = 3.70;
        public int Quantity { get; init; }

        public IProduct UpdateQuantity(int quantity)
        {
            return new Chocolate() { Quantity = quantity };
        }
    }
}
