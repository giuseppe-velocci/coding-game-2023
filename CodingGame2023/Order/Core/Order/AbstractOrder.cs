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

        public abstract void AddProduct(IProduct product, int quantity);
        public abstract void AddPayment(IPayment payment);
    }
}