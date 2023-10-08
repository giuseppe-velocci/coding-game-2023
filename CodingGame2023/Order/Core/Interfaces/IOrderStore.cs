using Core;

namespace Order.Core.Interfaces
{
    public interface IOrderStore
    {
        public OperationResult<Order.Order> GetOrder(Key id);
    }
}
