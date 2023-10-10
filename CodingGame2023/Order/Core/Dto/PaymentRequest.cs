namespace Order.Core.Dto
{
    public record class PaymentRequest
    {
        public string Name { get; set; } = string.Empty;

        public PaymentRequest()
        {
        }
    }
}