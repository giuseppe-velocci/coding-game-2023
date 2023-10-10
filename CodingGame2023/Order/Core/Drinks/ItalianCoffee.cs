using Order.Core.Interfaces;

namespace Order.Core.Drinks
{
    public class ItalianCoffee : IProduct
    {
        public string Name { get; } = "Italian Coffee";
        public double Price { get; } = 1.50;
        public int Quantity { get; init; }

        public IProduct UpdateQuantity(int quantity)
        {
            return new ItalianCoffee() { Quantity = quantity };
        }
    }
}
