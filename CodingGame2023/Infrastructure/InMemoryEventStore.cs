using Core;
using System.Collections.Concurrent;

namespace Infrastructure
{
    public class InMemoryEventStore<TAggregate> : IEventStore<TAggregate> where TAggregate : class
    {
        private readonly ConcurrentDictionary<Key, List<IEvent>> _store = new();
        private readonly string _aggregateName = typeof(TAggregate).Name;

        public IReadOnlyCollection<IEvent> GetEvents(Key id)
        {
            if (id is not null && _store.TryGetValue(id, out var events))
            {
                return events;
            }
            else
            {
                return Array.Empty<IEvent>().ToList();
            }
        }

        public OperationResult<None> Store(IEvent newEvent)
        {
            var aggregateKey = newEvent.Id;
            if (_store.TryGetValue(aggregateKey, out var record))
            {
                if (record.Count + 1 == newEvent.Version)
                {
                    record.Add(newEvent);
                    return OperationResult<None>.CreateSuccess();
                }
                else
                {
                    return OperationResult<None>.CreateFailure($"Unexpected version number on update of {_aggregateName} at {aggregateKey}");
                }
            }
            else
            {
                if (newEvent.Version == 1)
                {
                    if (_store.TryAdd(aggregateKey, new List<IEvent> { newEvent }))
                    {
                        return OperationResult<None>.CreateSuccess();
                    }
                    else
                    {
                        return OperationResult<None>.CreateFailure($"Cannot store a new aggregate for of {_aggregateName} at {aggregateKey} because it is already stored");
                    }
                }
                else
                {
                    return OperationResult<None>.CreateFailure($"Version number for a new event of {_aggregateName} at {aggregateKey} must be exactly 1");
                }
            }
        }
    }
}