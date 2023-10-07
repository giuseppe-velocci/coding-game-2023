using Core;
using Order.Core;
using Order.Service.Aggregate;
using Order.Service.Command;
using Order.Service.Event;

namespace Order.Service
{
    public class OrderCommandHandler : IService<IOrder>
    {
        private readonly IOrderAggregate _aggregate;

        public OrderCommandHandler(IOrderAggregate aggregate)
        {
            _aggregate = aggregate;
        }

        public OperationResult<Key> Handle(ICommand command)
        {
            var result = command switch
            {
                CreateOrderCommand x => _aggregate.Apply(Handle(x).Value),
                _ => OperationResult<Key>.CreateFailure("Invalid command")
            };

            return result;
        }

        private OperationResult<OrderCreatedEvent> Handle(CreateOrderCommand command)
        {
            if (command == null)
            {
                return OperationResult<OrderCreatedEvent>.CreateFailure($"{nameof(CreateOrderCommand)} cannot be null");
            }
            else
            {
                return OperationResult<OrderCreatedEvent>.CreateSuccess(new());
            }
        }
    }
}