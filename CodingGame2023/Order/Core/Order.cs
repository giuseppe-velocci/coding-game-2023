using Core;
using Order.Core.Interfaces;

namespace Order.Core
{
    public abstract class Order : IOrder
    {
        public IEnumerable<IProduct> Basket { get; } = new List<IProduct>();

        public Key Id { get; protected set; } = new();

        public abstract void AddProduct(IProduct product, int quantity);

        public abstract int GetTotalCost();

        public abstract void PayWithCard();

        public abstract void PayWithCash();
    }

    public class ActiveOrder : Order
    {
        public override void AddProduct(IProduct product, int quantity)
        {
            throw new NotImplementedException();
        }

        public override int GetTotalCost()
        {
            throw new NotImplementedException();
        }

        public override void PayWithCard()
        {
            throw new NotImplementedException();
        }

        public override void PayWithCash()
        {
            throw new NotImplementedException();
        }
    }
}