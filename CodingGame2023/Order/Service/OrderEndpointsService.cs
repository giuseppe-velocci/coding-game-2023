using Core;
using Order.Core.Dto;
using Order.Core.Interfaces;
using Order.Service.Commands;

namespace Order.Service
{
    public class OrderEndpointsService : IOrderEndpointsService
    {
        private readonly ICommandHandler<IOrder> _service;
        private readonly IProductStore _productStore;
        private readonly IOrderStore _orderStore;
        private readonly IPaymentStore _paymentStore;

        public OrderEndpointsService(ICommandHandler<IOrder> service, IProductStore productStore, IOrderStore orderStore, IPaymentStore paymentStore)
        {
            _service = service;
            _productStore = productStore;
            _orderStore = orderStore;
            _paymentStore = paymentStore;
        }

        public Task<OperationResult<Core.Order.Order>> GetOrderAsync(Key id)
        {
            return _orderStore.GetOrderAsync(id);
        }

        public Task<OperationResult<Key>> CreateOrderAsync()
        {
            return _service.HandleAsync(new CreateOrderCommand());
        }

        public Task<OperationResult<Key>> AddToBasketAsync(string order, ProductRequest product)
        {
            return _service.HandleAsync(new AddProductToBasketCommand(new Key(order), product.Name, product.Quantity));
        }

        public Task<OperationResult<Key>> AddPaymentAsync(string order, PaymentRequest payment)
        {
            return _service.HandleAsync(new AddPaymentCommand(new Key(order), payment.Name));
        }

        public Task<IEnumerable<IProduct>> GetDrinksAsync()
        {
            return _productStore.GetProductsAsync();
        }

        public Task<IEnumerable<string>> GetPaymentsAsync()
        {
            return _paymentStore.GetPaymentsAsync();
        }
    }
}
