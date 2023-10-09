using Core;

namespace Order.Core.Interfaces
{
    public interface IOrderStore
    {
        Task<OperationResult<Order.Order>> GetOrderAsync(Key id);
    }
}
