namespace Order.Api.Models
{
    public record class ProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }

        public ProductRequest()
        {
            
        }
    }
}
