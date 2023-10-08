namespace Order.Api.Models
{
    public record class PaymentRequest
    {
        public string Name { get; set; } = string.Empty;

        public PaymentRequest()
        {
        }
    }
}