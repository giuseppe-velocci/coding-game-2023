using Core;

namespace Order.Core.Interfaces
{
    public interface IOrderStore
    {
        Task<OperationResult<Order.AbstractOrder>> GetOrderAsync(Key id);
    }
}
