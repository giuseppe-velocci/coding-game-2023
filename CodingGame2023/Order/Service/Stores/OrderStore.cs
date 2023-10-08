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

        OperationResult<Core.Order.Order> IOrderStore.GetOrder(Key id)
        {
            IOrder? result = _aggregate.GetInstance(id);

            return result is null ?
                OperationResult<Core.Order.Order>.CreateFailure("Order not found for provided key") :
                OperationResult<Core.Order.Order>.CreateSuccess((Core.Order.Order)result);
        }
    }
}
