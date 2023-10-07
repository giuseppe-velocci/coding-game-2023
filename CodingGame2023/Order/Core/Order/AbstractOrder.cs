using Core;
using Order.Core.Interfaces;

namespace Order.Core.Order
{
    public abstract class AbstractOrder : IOrder
    {
        protected List<IProduct> Basket { get; } = new List<IProduct>();
        public Key Id { get; protected set; } = new();

        public abstract void AddProduct(IProduct product, int quantity);

        public abstract double GetTotalCost();

        public abstract void PayWithCard();

        public abstract void PayWithCash();

        public IEnumerable<IProduct> GetProducts() => Basket.ToArray();
    }
}