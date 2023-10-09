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

        public Task<OperationResult<IPayment>> FindAsync(string name)
        {
            var payment = _payments.FirstOrDefault(x => name == x.GetType().Name);

            return Task.FromResult(payment is null ?
                OperationResult<IPayment>.CreateFailure("Payment not found") :
                OperationResult<IPayment>.CreateSuccess(payment));
        }

        public Task<IEnumerable<string>> GetPaymentsAsync()
        {
            return Task.FromResult(_payments.Select(x => x.GetType().Name));
        }
    }
}
