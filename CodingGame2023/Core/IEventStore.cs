namespace Core
{
    public interface IEventStore<in TAggregate> where TAggregate : class
    {
        public Task<OperationResult<None>> StoreAsync(IEvent newEvent);
        public Task<IReadOnlyCollection<IEvent>> GetEventsAsync(Key id);
    }
}