using Core;
using Order.Core.Interfaces;

namespace Order.Core.Payments
{
    public class CardPayment : IPayment
    {
        public CardPayment(PaymentOutcome paymentOutcome)
        {
            PaymentOutcome = paymentOutcome;
        }

        public CardPayment()
        {
        }

        public Key OrderId { get; } = null!;
        public PaymentOutcome PaymentOutcome { get; } = PaymentOutcome.Unkown;

        public bool IsAllowed(double amount)
        {
            return true;
        }
    }
}
