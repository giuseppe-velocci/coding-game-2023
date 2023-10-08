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
            OperationResult<Key> result = command switch
            {
                CreateOrderCommand x => _aggregate.Apply(Handle(x).Value),
                AddProductToBasketCommand x => _aggregate.Apply(Handle(x).Value),
                _ => OperationResult<Key>.CreateFailure("Invalid command")
            };

            return result;
        }

        private OperationResult<OrderCreatedEvent> Handle(CreateOrderCommand command)
        {
            return command == null ?
                OperationResult<OrderCreatedEvent>.CreateFailure($"{nameof(CreateOrderCommand)} cannot be null") :
                OperationResult<OrderCreatedEvent>.CreateSuccess(new());
        }

        private OperationResult<ProductAddedToBasketEvent> Handle(AddProductToBasketCommand command)
        {
            if (command.Quantity > 0)
            {
                var productSearch = _productStore.Find(command.ProductName);

                return productSearch.Success ?
                    OperationResult<ProductAddedToBasketEvent>.CreateSuccess(new ProductAddedToBasketEvent(command.Id, productSearch.Value, command.Quantity)) :
                    OperationResult<ProductAddedToBasketEvent>.CreateFailure("Product not found");
            }
            else
            {
                return OperationResult<ProductAddedToBasketEvent>.CreateFailure("Invalid quantity");
            }
        }
    }
}
