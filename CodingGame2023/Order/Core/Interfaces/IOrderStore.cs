using Core;

namespace Order.Core.Interfaces
{
    internal interface IOrderStore
    {
        public OperationResult<IOrder> GetOrder(Key id);
    }
}
