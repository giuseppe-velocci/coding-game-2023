using Core;

namespace Order.Core.Interfaces
{
    public interface IPayment
    {
        PaymentOutcome PaymentOutcome { get; }
        PaymentType PaymentType { get; }
        public Key OrderId { get; }


        bool IsAllowed(double amount);
        IPayment UpdateOrderId(Key orderId);
    }

    public enum PaymentOutcome
    {
        Unkown,
        Failure,
        Success
    }

    public enum PaymentType
    {
        Cash,
        Card
    }
}