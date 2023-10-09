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

        public Task<OperationResult<IProduct>> FindAsync(string productName)
        {
            var product = _drinks.FirstOrDefault(x => x.Name == productName);

            return Task.FromResult(product is null ?
                OperationResult<IProduct>.CreateFailure("Product not found") :
                OperationResult<IProduct>.CreateSuccess(product));
        }

        public Task<IEnumerable<IProduct>> GetProductsAsync()
        {
            return Task.FromResult(_drinks.ToArray() as IEnumerable<IProduct>);
        }
    }
}
