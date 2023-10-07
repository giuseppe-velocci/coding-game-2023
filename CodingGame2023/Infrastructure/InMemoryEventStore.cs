using Core;
using System.Collections.Concurrent;

namespace Infrastructure
{
    public class InMemoryEventStore<TAggregate> : IEventStore<TAggregate> where TAggregate : class
    {
        private readonly ConcurrentDictionary<Key, List<IEvent>> _store = new();
        private readonly string _aggregateName = typeof(TAggregate).Name;

        public OperationResult<None> Store(Key aggregateKey, IEvent currentEvent)
        {
            if (_store.TryGetValue(aggregateKey, out var record))
            {
                if (record.Count == currentEvent.Version)
                {
                    record.Add(currentEvent);
                    return OperationResult<None>.CreateSuccess();
                }
                else
                {
                    return OperationResult<None>.CreateFailure($"Unexpected version number on update of {_aggregateName} at {aggregateKey}");
                }
            }
            else
            {
                if (currentEvent.Version != 1) 
                {
                    return OperationResult<None>.CreateFailure($"Version number for a new event of {_aggregateName} at {aggregateKey} must be exactly 1");
                }
                else
                {
                    if (_store.TryAdd(aggregateKey, new List<IEvent> { currentEvent }))
                    {
                        return OperationResult<None>.CreateSuccess();
                    }
                    else
                    {
                        return OperationResult<None>.CreateFailure($"Cannot store a new aggregate for of {_aggregateName} at {aggregateKey} because it is already stored");
                    }
                }
            }
            
        }
    }
}