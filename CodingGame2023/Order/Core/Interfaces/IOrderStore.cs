using Core;

namespace Order.Core.Interfaces
{
    public interface IOrderStore
    {
        public OperationResult<IOrder> GetOrder(Key id);
    }
}
