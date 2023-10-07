using Core;

namespace Order.Core.Interfaces
{
    public interface IProductStore
    {
        IEnumerable<IProduct> GetProducts();
        OperationResult<IProduct> Find(string productName);
    }
}