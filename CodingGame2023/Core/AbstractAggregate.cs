namespace Core
{
    public abstract class AbstractAggregate<T> : IAggregate<T> where T : class
    {
        public T Instance { get; protected set; } = null!;
        protected readonly IEventStore<T> _eventStore;

        protected AbstractAggregate(IEventStore<T> eventStore)
        {
            _eventStore = eventStore;
        }

        public OperationResult<Key> Apply(IEvent newEvent)
        {
            var newVersionResult = ApplyAllChanges(newEvent.Id);
            if (newVersionResult.Success)
            {
                var result = ApplyChange(newEvent.UpdateVersion(newVersionResult.Value));
                if (result.Success)
                {
                    _eventStore.Store(newEvent);
                }
                return result;
            }
            else
            {
                return OperationResult<Key>.CreateFailure("Concurrent update failure");
            }
        }

        protected abstract OperationResult<Key> ApplyChange(IEvent storedEvent);

        protected OperationResult<int> ApplyAllChanges(Key id)
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
