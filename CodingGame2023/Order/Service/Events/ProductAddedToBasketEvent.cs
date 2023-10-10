using Core;
using Order.Core.Interfaces;

namespace Order.Service.Events
{
    public class ProductAddedToBasketEvent : IEvent
    {
        public ProductAddedToBasketEvent(Key id, IProduct product, int quantity)
        {
            Id = id;
            Product = product;
            Quantity = quantity;
        }

        public IEvent UpdateVersion(int version)
        {
            return new ProductAddedToBasketEvent(Id, Product, Quantity) { Version = version };
        }

        public Key Id { get; }
        public string Name => nameof(ProductAddedToBasketEvent);
        public int Version { get; private init; }
        public IProduct Product { get; }
        public int Quantity { get; }
    }
}