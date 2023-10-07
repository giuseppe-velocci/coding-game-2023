using Core;
using Order.Core.Interfaces;

namespace Order.Service.Events
{
    public class ProductAddedToBaketEvent : IEvent
    {
        public ProductAddedToBaketEvent(Key id, IProduct product, int quantity)
        {
            Id = id;
            Product = product;
            Quantity = quantity;
        }

        public ProductAddedToBaketEvent UpdateVersion(int version)
        {
            return new ProductAddedToBaketEvent(Id, Product, Quantity) { Version = version };
        }

        public Key Id { get; }
        public string Name => nameof(ProductAddedToBaketEvent);
        public int Version { get; private init; }
        public IProduct Product { get; }
        public int Quantity { get; }
    }
}