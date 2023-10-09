using Core;

namespace Order.Core.Interfaces
{
    public interface IPaymentStore
    {
        Task<OperationResult<IPayment>> FindAsync(string name);
        Task<IEnumerable<string>> GetPaymentsAsync();
    }
}
