using Core;

namespace Order.Core.Interfaces
{
    public interface IOrder
    {
        Key Id { get; }
        OperationResult<Key> AddPayment(IPayment payment);
        OperationResult<Key> AddProduct(IProduct product, int quantity);
        IPayment? GetPayment();
        IEnumerable<IProduct> GetProducts();
        double GetTotalAmount();
        void RemoveProduct(IProduct product);
    }
}