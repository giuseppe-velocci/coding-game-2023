﻿using Core;
using Order.Core;
using Order.Service.Aggregate;
using Order.Service.Event;

namespace Order.Service.Aggregate
{
    internal sealed class OrderAggregate : IOrderAggregate
    {
        public IOrder Instance { get; private set; } = null!;

        private readonly IEventStore<IOrder> _eventStore;

        public OrderAggregate(IEventStore<IOrder> eventStore)
        {
            _eventStore = eventStore;
        }

        public OperationResult<Key> Apply(OrderCreatedEvent orderCreatedEvent)
        {
            if (Instance == null)
            {
                Instance = new ActiveOrder();
                return OperationResult<Key>.CreateSuccess(Instance.Id);
            }
            else
            {
                return OperationResult<Key>.CreateFailure($"{nameof(OrderCreatedEvent)} must be invoked on a new {nameof(IOrder)} instance");
            }
        }
    }
}