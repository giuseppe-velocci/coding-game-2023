using Core;

namespace Order.Core
{
    public interface IOrder
    {
        Key Id { get; }
        void AddProduct(IProduct product, int quantity);
        int GetTotalCost();
        void PayWithCash();
        void PayWithCard();
    }
}