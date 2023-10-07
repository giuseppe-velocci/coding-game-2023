using Core;
using Order.Core.Interfaces;
using Order.Service.Aggregates;
using Order.Service.Commands;
using Order.Service.Events;

namespace Order.Service.Handlers
{
    public class OrderCommandHandler : ICommandHandler<IOrder>
    {
        private readonly IOrderAggregate _aggregate;
        private readonly IProductStore _productStore;

        public OrderCommandHandler(IOrderAggregate aggregate, IProductStore productStore)
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

        private OperationResult<ProductAddedToBaketEvent> Handle(AddProductToBasketCommand command)
        {
            var productSearch = _productStore.Find(command.ProductName);

            return productSearch.Success ?
                OperationResult<ProductAddedToBaketEvent>.CreateSuccess(new ProductAddedToBaketEvent(command.Id, productSearch.Value, command.Quantity)) :
                OperationResult<ProductAddedToBaketEvent>.CreateFailure("Product not found");
        }
    }
}
