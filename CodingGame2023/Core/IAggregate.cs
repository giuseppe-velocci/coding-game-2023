namespace Core
{
    public interface IAggregate<T> where T : class
    {
        Task<T?> GetInstance(Key id);
        Task<OperationResult<Key>> Apply(IEvent newEvent);
    }
}
