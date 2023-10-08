using Core;
using Order.Core.Interfaces;
using Order.Service.Commands;
using Order.Service.Events;

namespace Order.Service.CommandHandlers
{
    public class OrderCommandHandler : ICommandHandler<IOrder>
    {
        private readonly IAggregate<IOrder> _aggregate;
        private readonly IProductStore _productStore;
        private readonly IPaymentStore _paymentStore;

        public OrderCommandHandler(IAggregate<IOrder> aggregate, IProductStore productStore, IPaymentStore paymentStore)
        {
            _aggregate = aggregate;
            _productStore = productStore;
            _paymentStore = paymentStore;
        }

        public OperationResult<Key> Handle(ICommand command)
        {
            var eventfromCommand = command switch
            {
                CreateOrderCommand x => Handle(x),
                AddProductToBasketCommand x => Handle(x),
                AddPaymentCommand x => Handle(x),
                _ => OperationResult<IEvent>.CreateFailure("Invalid command")
            };

            return eventfromCommand.Success ?
                _aggregate.Apply(eventfromCommand.Value) :
                OperationResult<Key>.CreateFailure(eventfromCommand.Message);
        }

        private static OperationResult<IEvent> Handle(CreateOrderCommand command)
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

        private OperationResult<IEvent> Handle(AddPaymentCommand command)
        {
            var payment = _paymentStore.Find(command.Payment);

            return payment.Success ?
                OperationResult<IEvent>.CreateSuccess(new PaymentAddedEvent(command.Id, payment.Value.UpdateOrderId(command.Id))) :
                OperationResult<IEvent>.CreateFailure("Invalid payment method");
        }
    }
}
