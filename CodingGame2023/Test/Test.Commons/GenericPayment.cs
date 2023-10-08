using Core;
using Order.Core.Interfaces;

namespace Test.Commons
{
    public class GenericPayment : IPayment
    {
        public GenericPayment(Key orderId)
        {
            OrderId = orderId;
        }

        public PaymentOutcome PaymentOutcome => PaymentOutcome.Unkown;
        public Key OrderId { get; }

        public PaymentType PaymentType => PaymentType.Cash;

        public bool IsAllowed(double amount) => true;

        public IPayment UpdateOrderId(Key orderId) => new GenericPayment(orderId);
    }
}
