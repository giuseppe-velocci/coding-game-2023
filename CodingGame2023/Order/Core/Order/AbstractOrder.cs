﻿using Core;
using Order.Core.Interfaces;

namespace Order.Core.Order
{
    public abstract class AbstractOrder : IOrder
    {
        public List<IProduct> Basket { get; } = new List<IProduct>();
        public Key Id { get; protected set; } = new();
        public IPayment? Payment { get; protected set; } = null!;

        public IEnumerable<IProduct> GetProducts() => Basket.ToArray();
        public IPayment? GetPayment() => Payment;
        public double GetTotalAmount() => Basket.Select(x => x.Quantity * x.Price).Sum();

        public abstract OperationResult<Key> AddProduct(IProduct product, int quantity);
        public abstract OperationResult<Key> AddPayment(IPayment payment);

        public void RemoveProduct(IProduct product)
        {
            IProduct? existingProduct = Basket.FirstOrDefault(x => x.Name == product.Name);
            if (existingProduct != null)
            {
                Basket.Remove(existingProduct);
            }
        }
    }
}