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
        { }

        public Key OrderId { get; private init; } = null!;
        public PaymentOutcome PaymentOutcome { get; } = PaymentOutcome.Unkown;
        public PaymentType PaymentType => PaymentType.Cash;

        public bool IsAllowed(double amount)
        {
            return amount <= 10.0;
        }

        public IPayment UpdateOrderId(Key orderId)
        {
            return new CashPayment() { OrderId = orderId };
        }
    }
}
