using System.Threading.Tasks;

namespace Core
{
    public interface IEventStore<in TAggregate> where TAggregate : class
    {
        public Task StoreAsync(IEvent currentEvent);
    }
}