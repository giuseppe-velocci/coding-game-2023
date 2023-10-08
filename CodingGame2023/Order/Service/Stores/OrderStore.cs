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
        public OperationResult<IOrder> GetOrder(Key id)
        {
            IOrder? result = _aggregate.GetInstance(id);

            return result is null ?
                OperationResult<IOrder>.CreateFailure("Order not found for provided key") :
                OperationResult<IOrder>.CreateSuccess(result);

        }

    }
}
