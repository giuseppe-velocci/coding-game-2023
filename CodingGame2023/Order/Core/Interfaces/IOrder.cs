using Core;

namespace Order.Core.Interfaces
{
    public interface IOrder
    {
        Key Id { get; }
        void AddProduct(IProduct product, int quantity);
        void RemoveProduct(IProduct product);
        double GetTotalAmount();
        void AddPayment(IPayment payment);
        IEnumerable<IProduct> GetProducts();
        IPayment? GetPayment();
    }
}