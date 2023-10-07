namespace Core
{
    public interface IEventStore<in TAggregate> where TAggregate : class
    {
        public OperationResult<None> Store(IEvent newEvent);
        public IReadOnlyCollection<IEvent> GetEvents(Key id);
    }
}