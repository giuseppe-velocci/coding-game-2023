using Core;
using Order.Core.Interfaces;

namespace Order.Api
{
    public interface IProductStore
    {
        IEnumerable<IProduct> GetProducts();
        OperationResult<IProduct> Find(string productName);
    }
}