using Core;
using Order.Core.Interfaces;
using Order.Core.Payments;

namespace Order.Service.Stores
{
    internal class PaymentStore : IPaymentStore
    {
        private readonly IPayment[] _payments = new IPayment[]
        {
            new CardPayment(),
            new CashPayment(),
        };

        public OperationResult<IPayment> GetPayment(string name)
        {
            var payment = _payments.FirstOrDefault(x => name == x.GetType().Name);

            return payment is null ?
                OperationResult<IPayment>.CreateFailure("Payment not found") :
                OperationResult<IPayment>.CreateSuccess(payment);
        }

        public IEnumerable<string> GetPayments()
        {
            return _payments.Select(x => x.GetType().Name);
        }
    }
}
