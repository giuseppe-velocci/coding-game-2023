using Core;
using Order.Core.Interfaces;
using Order.Core.Order;
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
                IProduct? existingProduct = Instance.GetProducts().FirstOrDefault(x => x.Name == newEvent.Product.Name);
                if (existingProduct is not null)
                {
                    Instance.RemoveProduct(existingProduct);
                }

                return Instance.AddProduct(newEvent.Product, newEvent.Quantity);
            }
        }

        private OperationResult<Key> Apply(PaymentAddedEvent newEvent)
        {
            if (Instance is null)
            {
                return OperationResult<Key>.CreateFailure("Order does not exists");
            }
            else if (!Instance.GetProducts().Any())
            {
                return OperationResult<Key>.CreateFailure("Cannot checkout an Order without products");
            }
            else if (newEvent.Payment.IsAllowed(Instance.GetTotalAmount()))
            {
                Instance = new ClosedOrder(Instance);
                var addPaymentResult = Instance.AddPayment(newEvent.Payment);
                if (addPaymentResult.Success)
                {
                    Instance = new ClosedOrder(Instance);
                    return OperationResult<Key>.CreateSuccess(newEvent.Id);
                }
                else
                {
                    return addPaymentResult;
                }
            }
            else
            {
                return OperationResult<Key>.CreateFailure("Selected payment method unavailable for this amount");
            }
        }
    }
}
