using Core;
using Order.Core.Interfaces;

namespace Order.Core.Order
{
    public class ClosedOrder : AbstractOrder
    {
        public ClosedOrder(IOrder order)
        {
            Basket.AddRange(order.GetProducts());
            Id = order.Id;
        }

        public override OperationResult<Key> AddPayment(IPayment payment)
        {
            Payment = payment;
            return OperationResult<Key>.CreateSuccess(Id);
        }

        public override OperationResult<Key> AddProduct(IProduct product, int quantity)
        {
            return OperationResult<Key>.CreateFailure("Canot add a product for a closed Order");

        }
    }
}