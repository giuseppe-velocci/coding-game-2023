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

        public bool IsAllowed(double amount) => true;
    }
}
