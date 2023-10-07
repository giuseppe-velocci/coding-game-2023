using Core;
using Order.Core.Interfaces;
using Order.Service.Events;

namespace Order.Service.Aggregates
{
    public interface IOrderAggregate : IAggregate<IOrder>
    {
        OperationResult<Key> Apply(OrderCreatedEvent newEvent);
        OperationResult<Key> Apply(ProductAddedToBaketEvent newEvent);
    }
}
