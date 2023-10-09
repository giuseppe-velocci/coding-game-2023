using Core;
using Order.Core.Dto;

namespace Order.Core.Interfaces
{
    public interface IOrderEndpointsService
    {
        Task<OperationResult<Key>> AddPaymentAsync(string order, PaymentRequest payment);
        Task<OperationResult<Key>> AddToBasketAsync(string order, ProductRequest product);
        Task<OperationResult<Key>> CreateOrderAsync();
        Task<IEnumerable<IProduct>> GetDrinksAsync();
        Task<OperationResult<Order.Order>> GetOrderAsync(Key id);
        Task<IEnumerable<string>> GetPaymentsAsync();
    }
}