namespace Core
{
    public interface IAggregate<T> where T : class
    {
        T Instance { get; }
        OperationResult<Key> Apply(IEvent newEvent);
    }
}
