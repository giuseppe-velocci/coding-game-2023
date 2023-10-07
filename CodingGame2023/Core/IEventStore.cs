using System.Threading.Tasks;

namespace Core
{
    public interface IEventStore<in TAggregate> where TAggregate : class
    {
        public OperationResult<None> Store(Key aggregateKey, IEvent currentEvent);
    }
}