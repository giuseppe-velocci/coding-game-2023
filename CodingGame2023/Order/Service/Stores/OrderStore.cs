using Core;
using Order.Core.Interfaces;

namespace Order.Service.Stores
{
    internal class OrderStore : IOrderStore
    {
        private readonly IAggregate<IOrder> _aggregate;

        public OrderStore(IAggregate<IOrder> aggregate)
        {
            _aggregate = aggregate;
        }

        async Task<OperationResult<Core.Order.AbstractOrder>> IOrderStore.GetOrderAsync(Key id)
        {
            IOrder? result = await _aggregate.GetInstance(id);

            return result is null ?
                OperationResult<Core.Order.AbstractOrder>.CreateFailure("Order not found for provided key") :
                OperationResult<Core.Order.AbstractOrder>.CreateSuccess((Core.Order.AbstractOrder)result);
        }
    }
}
