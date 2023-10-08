using Core;
using Order.Core.Interfaces;
using Order.Service.Events;

namespace Order.Service.Aggregates
{
    internal sealed class OrderAggregate : AbstractAggregate<IOrder>
    {
        public OrderAggregate(IEventStore<IOrder> eventStore) : base(eventStore)
        { }

        protected override OperationResult<Key> ApplyChange(IEvent storedEvent)
        {
            OperationResult<Key> result = storedEvent switch
            {
                OrderCreatedEvent x => Apply(x),
                ProductAddedToBasketEvent x => Apply(x),
                PaymentAddedEvent x => Apply(x),
                _ => OperationResult<Key>.CreateFailure("Invalid event")
            };
            return result;
        }

        private OperationResult<Key> Apply(OrderCreatedEvent newEvent)
        {
            if (Instance is null)
            {
                Instance = new Core.Order.Order(newEvent.Id);
                return OperationResult<Key>.CreateSuccess(newEvent.Id);
            }
            else
            {
                return OperationResult<Key>.CreateFailure("Cannot create a new Order that already exists");
            }
        }

        private OperationResult<Key> Apply(ProductAddedToBasketEvent newEvent)
        {
            if (Instance is null)
            {
                return OperationResult<Key>.CreateFailure("Order does not exists");
            }
            else
            {
                Instance.AddProduct(newEvent.Product, newEvent.Quantity);
                return OperationResult<Key>.CreateSuccess(newEvent.Id);
            }
        }

        private OperationResult<Key> Apply(PaymentAddedEvent newEvent)
        {
            if (newEvent.Payment.IsAllowed(Instance.GetTotalAmount()))
            {
                Instance.AddPayment(newEvent.Payment);
                return OperationResult<Key>.CreateSuccess(newEvent.Id);
            }
            else
            {
                return OperationResult<Key>.CreateFailure("Selected payment method unavailable for this amount");
            }
        }
    }
}
