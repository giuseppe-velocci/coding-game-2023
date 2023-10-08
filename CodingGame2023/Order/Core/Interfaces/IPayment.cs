﻿using Core;

namespace Order.Core.Interfaces
{
    public interface IPayment
    {
        PaymentOutcome PaymentOutcome { get; }
        public Key OrderId { get; }
        bool IsAllowed(double amount);
    }

    public enum PaymentOutcome
    {
        Unkown,
        Failure,
        Success
    }
}