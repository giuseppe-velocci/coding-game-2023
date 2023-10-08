using Core;
using Order.Api.Models;
using Order.Core.Interfaces;
using Order.Service.Commands;

namespace Order.Api
{
    public class OrderEndpoints
    {
        private readonly ICommandHandler<IOrder> _service;
        private readonly IProductStore _productStore;

        public OrderEndpoints(ICommandHandler<IOrder> service, IProductStore productStore)
        {
            _service = service;
            _productStore = productStore;
        }

        public Key GetOrder(Key id)
        {
            return id;
        }


        public OperationResult<Key> CreateOrder()
        {
            return _service.Handle(new CreateOrderCommand());
        }

        public OperationResult<Key> AddToBasket(string order, ProductRequest product)
        {
            return _service.Handle(new AddProductToBasketCommand(new Key(order), product.Name, product.Quantity));
        }

        public OperationResult<Key> AddPayment(string order, PaymentRequest payment)
        {
            return _service.Handle(new AddPaymentCommand(new Key(order), payment.Name));
        }

        public IEnumerable<IProduct> GetDrinks()
        {
            return _productStore.GetProducts();
        }
    }
}
