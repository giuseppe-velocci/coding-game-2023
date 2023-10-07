using Core;

namespace Order.Service.Events
{
    public class OrderCreatedEvent : IEvent
    {
        public Key Id { get; } = new Key();
        public string Name => nameof(OrderCreatedEvent);
        public int Version => 1;
    }
}
