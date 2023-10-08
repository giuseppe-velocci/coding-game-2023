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

        public override void AddPayment(IPayment payment)
        {
            Payment = payment;
        }

        public override void AddProduct(IProduct product, int quantity)
        {
            if (product is not null)
            {
                Basket.Add(product.UpdateQuantity(quantity));
            }
        }
    }
}