using Core;

namespace Order.Service.Event
{
    public class OrderCreatedEvent
    {
        public Key Id { get; } = new Key();
    }
}
