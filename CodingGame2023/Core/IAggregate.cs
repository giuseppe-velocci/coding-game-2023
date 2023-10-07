namespace Core
{
    public interface IAggregate<T> where T : class
    {
        T Instance { get; }
    }
}
