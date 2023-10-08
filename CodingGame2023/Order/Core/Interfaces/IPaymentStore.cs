using Core;

namespace Order.Core.Interfaces
{
    public interface IPaymentStore
    {
        OperationResult<IPayment> GetPayment(string name);
    }
}
