using Core;

namespace Order.Service.Commands
{
    public class AddProductToBasketCommand : ICommand
    {
        public AddProductToBasketCommand(Key id, string productName, int quantity)
        {
            Id = id;
            ProductName = productName;
            Quantity = quantity;
        }

        public Key Id { get; set; }
        public string ProductName { get; } = string.Empty;
        public int Quantity { get; }
    }
}
