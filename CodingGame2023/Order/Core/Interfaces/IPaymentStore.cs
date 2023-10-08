using Core;

namespace Order.Core.Interfaces
{
    public interface IPaymentStore
    {
        OperationResult<IPayment> Find(string name);
        IEnumerable<string> GetPayments();
    }
}
