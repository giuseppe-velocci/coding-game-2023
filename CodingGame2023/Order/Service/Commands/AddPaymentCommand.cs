using Core;
using Order.Core.Interfaces;

namespace Order.Service.Commands
{
    public class AddPaymentCommand : ICommand
    {
        public AddPaymentCommand(Key id, string paymentName)
        {
            Id = id;
            Payment = paymentName;
        }

        public Key Id { get; }
        public string Payment { get; }
    }
}
