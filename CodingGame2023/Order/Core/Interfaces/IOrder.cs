using Core;

namespace Order.Core.Interfaces
{
    public interface IOrder
    {
        Key Id { get; }
        void AddProduct(IProduct product, int quantity);
        double GetTotalCost();
        void PayWithCash();
        void PayWithCard();
        IEnumerable<IProduct> GetProducts();
    }
}