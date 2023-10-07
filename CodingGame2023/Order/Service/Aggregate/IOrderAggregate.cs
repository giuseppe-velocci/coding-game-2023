using Core;
using Order.Core;
using Order.Service.Event;

namespace Order.Service.Aggregate
{
    public interface IOrderAggregate : IAggregate<IOrder>
    {
        OperationResult<Key> Apply(OrderCreatedEvent orderCreatedEvent);
    }
}
