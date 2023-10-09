using Core;

namespace Order.Core.Interfaces
{
    public interface IProductStore
    {
        Task<IEnumerable<IProduct>> GetProductsAsync();
        Task<OperationResult<IProduct>> FindAsync(string productName);
    }
}