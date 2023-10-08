using Core;
using Order.Core.Drinks;
using Order.Core.Interfaces;

namespace Order.Service.Stores
{
    internal class ProductStore : IProductStore
    {
        private readonly IProduct[] _drinks = new IProduct[]
        {
            new AmericanCoffee(),
            new ItalianCoffee(),
            new Tea(),
            new Chocolate()
        };

        public OperationResult<IProduct> Find(string productName)
        {
            var product = _drinks.FirstOrDefault(x => x.Name == productName);

            return product is null ?
                OperationResult<IProduct>.CreateFailure("Product not found") :
                OperationResult<IProduct>.CreateSuccess(product);
        }

        public IEnumerable<IProduct> GetProducts()
        {
            return _drinks.ToArray();
        }
    }
}
