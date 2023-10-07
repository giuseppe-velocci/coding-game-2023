using Core;
using Order.Api.Models;
using Order.Core.Drinks;
using Order.Core.Interfaces;
using Order.Service.Commands;

namespace Order.Api
{
    public class OrderEndpoints
    {
        private readonly ICommandHandler<IOrder> _service;

        public OrderEndpoints(ICommandHandler<IOrder> service)
        {
            _service = service;
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

        public IEnumerable<IProduct> GetDrinks()
        {
            return new IProduct[]
            {
                new AmericanCoffee(),
                new ItalianCoffee(),
                new Tea(),
                new Chocolate()
            };
        }
    }
}
