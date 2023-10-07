using Core;
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
    }
}
