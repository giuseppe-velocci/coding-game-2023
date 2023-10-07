using Core;
using Order.Core.Interfaces;

namespace Order.Core.Order
{
    public class Order : AbstractOrder
    {
        public Order(Key id)
        {
            Id = id;
        }

        public override void AddProduct(IProduct product, int quantity)
        {
            if (product is not null)
            {
                Basket.Add(product.UpdateQuantity(quantity));
            }
        }

        public override double GetTotalCost()
        {
            return Basket.Select(x => x.Quantity * x.Price).Sum();
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