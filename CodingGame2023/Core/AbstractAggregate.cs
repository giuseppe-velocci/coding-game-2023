﻿namespace Core
{
    public abstract class AbstractAggregate<T> : IAggregate<T> where T : class
    {
        protected T Instance { get; set; } = null!;
        protected readonly IEventStore<T> _eventStore;

        protected AbstractAggregate(IEventStore<T> eventStore)
        {
            _eventStore = eventStore;
        }

        public T? GetInstance(Key id)
        {
            ApplyAllChanges(id);
            return Instance;
        }

        public OperationResult<Key> Apply(IEvent newEvent)
        {
            if (newEvent is null)
            {
                return OperationResult<Key>.CreateFailure("Invalid event");
            }

            var newVersionResult = ApplyAllChanges(newEvent.Id);
            if (newVersionResult.Success)
            {
                var versionedEvent = newEvent.UpdateVersion(newVersionResult.Value);
                var result = ApplyChange(versionedEvent);
                if (result.Success)
                {
                    _eventStore.Store(versionedEvent);
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
