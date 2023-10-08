namespace Core
{
    public interface IAggregate<T> where T : class
    {
        T? GetInstance(Key id);
        OperationResult<Key> Apply(IEvent newEvent);
    }
}
