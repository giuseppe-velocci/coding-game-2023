using Core;
using Order.Core.Interfaces;
using Order.Service.Aggregates;
using Order.Service.Commands;
using Order.Service.Events;

namespace Order.Service
{
    public class OrderCommandHandler : ICommandHandler<IOrder>
    {
        private readonly IOrderAggregate _aggregate;

        public OrderCommandHandler(IOrderAggregate aggregate)
        {
            _aggregate = aggregate;
        }

        public OperationResult<Key> Handle(ICommand command)
        {
            OperationResult<Key> result = command switch
            {
                CreateOrderCommand x => _aggregate.Apply(Handle(x).Value),
                _ => OperationResult<Key>.CreateFailure("Invalid command")
            };

            return result;
        }

        private OperationResult<OrderCreatedEvent> Handle(CreateOrderCommand command)
        {
            return command == null ?
                OperationResult<OrderCreatedEvent>.CreateFailure($"{nameof(CreateOrderCommand)} cannot be null") :
                OperationResult<OrderCreatedEvent>.CreateSuccess(new());
        }
    }
}