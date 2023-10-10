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

        public override OperationResult<Key> AddPayment(IPayment payment)
        {
            return OperationResult<Key>.CreateFailure("Cannot add a payment for an active Order");
        }

        public override OperationResult<Key> AddProduct(IProduct product, int quantity)
        {
            if (product is not null)
            {
                Basket.Add(product.UpdateQuantity(quantity));
                return OperationResult<Key>.CreateSuccess(Id);
            }
            else
            {
                return OperationResult<Key>.CreateFailure("Invalid Product");
            }
        }
    }
}