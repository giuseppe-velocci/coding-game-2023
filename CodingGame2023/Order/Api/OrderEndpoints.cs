using Core;
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

        public IEnumerable<IProduct> GetDrinks()
        {
            return new IProduct[]
            {
                new AmericanCoffee(0),
                new ItalianCoffee(0),
                new Tea(0),
                new Chocolate(0)
            };
        }

        public OperationResult<Key> CreateOrder()
        {
            return _service.Handle(new CreateOrderCommand());
        }
    }
}
