using Order.Core.Interfaces;

namespace Order.Core.Drinks
{
    public class AmericanCoffee : IProduct
    {
        public string Name { get; } = "American Coffee";
        public double Price { get; } = 2.50;
        public int Quantity { get; init; }

        public IProduct UpdateQuantity(int quantity)
        {
            return new AmericanCoffee() { Quantity = quantity };
        }
    }
}
