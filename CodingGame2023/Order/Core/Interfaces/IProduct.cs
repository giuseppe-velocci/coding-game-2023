namespace Order.Core.Interfaces
{
    public interface IProduct
    {
        string Name { get; }
        double Price { get; }
        int Quantity { get; init; }

        IProduct UpdateQuantity(int quantity);
    }
}