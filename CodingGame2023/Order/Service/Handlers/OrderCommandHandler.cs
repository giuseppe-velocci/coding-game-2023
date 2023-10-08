using Core;
using Order.Core.Interfaces;
using Order.Service.Commands;
using Order.Service.Events;

namespace Order.Service.Handlers
{
    public class OrderCommandHandler : ICommandHandler<IOrder>
    {
        private readonly IAggregate<IOrder> _aggregate;
        private readonly IProductStore _productStore;

        public OrderCommandHandler(IAggregate<IOrder> aggregate, IProductStore productStore)
        {
            _aggregate = aggregate;
            _productStore = productStore;
        }

        public OperationResult<Key> Handle(ICommand command)
        {
            var eventfromCommand = command switch
            {
                CreateOrderCommand x => Handle(x),
                AddProductToBasketCommand x => Handle(x),
                _ => OperationResult<IEvent>.CreateFailure("Invalid command")
            };

            return eventfromCommand.Success ?
                _aggregate.Apply(eventfromCommand.Value) :
                OperationResult<Key>.CreateFailure(eventfromCommand.Message);
        }

        private OperationResult<IEvent> Handle(CreateOrderCommand command)
        {
            return command == null ?
                OperationResult<IEvent>.CreateFailure($"{nameof(CreateOrderCommand)} cannot be null") :
                OperationResult<IEvent>.CreateSuccess(new OrderCreatedEvent());
        }

        private OperationResult<IEvent> Handle(AddProductToBasketCommand command)
        {
            if (command.Quantity > 0)
            {
                var productSearch = _productStore.Find(command.ProductName);

                return productSearch.Success ?
                    OperationResult<IEvent>.CreateSuccess(new ProductAddedToBasketEvent(command.Id, productSearch.Value, command.Quantity)) :
                    OperationResult<IEvent>.CreateFailure("Product not found");
            }
            else
            {
                return OperationResult<IEvent>.CreateFailure("Invalid quantity");
            }
        }
    }
}
