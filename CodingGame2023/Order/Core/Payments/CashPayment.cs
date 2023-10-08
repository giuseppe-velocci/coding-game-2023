using Core;
using Order.Core.Interfaces;

namespace Order.Core.Payments
{
    public class CashPayment : IPayment
    {
        public CashPayment(PaymentOutcome paymentOutcome)
        {
            PaymentOutcome = paymentOutcome;
        }

        public CashPayment()
        {
        }

        public Key OrderId { get; } = null!;
        public PaymentOutcome PaymentOutcome { get; } = PaymentOutcome.Unkown;

        public bool IsAllowed(double amount)
        {
            return amount <= 10.0;
        }
    }
}
