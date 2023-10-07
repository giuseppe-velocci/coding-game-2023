using Core;
using Order.Core.Interfaces;
using Order.Service.Events;

namespace Order.Service.Aggregates
{
    internal sealed class OrderAggregate : IOrderAggregate
    {
        public IOrder Instance { get; private set; } = null!;

        private readonly IEventStore<IOrder> _eventStore;

        public OrderAggregate(IEventStore<IOrder> eventStore)
        {
            _eventStore = eventStore;
        }

        public OperationResult<Key> Apply(OrderCreatedEvent newEvent)
        {
            if (Instance == null)
            {
                Instance = new Core.Order.Order(newEvent.Id);
                _eventStore.Store(newEvent);
                return OperationResult<Key>.CreateSuccess(Instance.Id);
            }
            else
            {
                return OperationResult<Key>.CreateFailure($"{nameof(OrderCreatedEvent)} must be invoked on a new {nameof(IOrder)} instance");
            }
        }

        public OperationResult<Key> Apply(ProductAddedToBaketEvent newEvent)
        {
            var newVersionResult = ApplyAllChanges(newEvent.Id);
            if (newVersionResult.Success)
            {
                Instance.AddProduct(newEvent.Product, newEvent.Quantity);
                newEvent.UpdateVersion(newVersionResult.Value);
                _eventStore.Store(newEvent);
                return OperationResult<Key>.CreateSuccess(Instance.Id);
            }
            else
            {
                return OperationResult<Key>.CreateFailure("Concurrent update failure");
            }
        }

        private OperationResult<Key> ApplyChange(IEvent storedEvent)
        {
            OperationResult<Key> result = storedEvent switch
            {
                OrderCreatedEvent x => Apply(x),
                ProductAddedToBaketEvent x => Apply(x),
                _ => OperationResult<Key>.CreateFailure("Invalid event")
            };
            return result;
        }

        private OperationResult<int> ApplyAllChanges(Key id)
        {
            var events = _eventStore.GetEvents(id);
            var results = events.Select(x => ApplyChange(x));
            if (results.Any(x => !x.Success))
            {
                return OperationResult<int>.CreateFailure("Invalid events in the chain");
            }
            else
            {
                int count = events.Count;
                return OperationResult<int>.CreateSuccess(count);
            }
        }
    }
}
