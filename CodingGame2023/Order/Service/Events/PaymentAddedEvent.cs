using Core;
using Order.Core.Interfaces;

namespace Order.Service.Events
{
    public class PaymentAddedEvent : IEvent
    {
        public PaymentAddedEvent(Key id, IPayment payment)
        {
            Id = id;
            Payment = payment;
        }
        public IPayment Payment { get; }

        public Key Id { get; } = new Key();
        public string Name => nameof(PaymentAddedEvent);
        public int Version { get; private init; }

        public IEvent UpdateVersion(int version)
        {
            return new PaymentAddedEvent(Id, Payment) { Version = version };
        }
    }
}
