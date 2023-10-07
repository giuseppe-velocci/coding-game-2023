namespace Order.Api.Models
{
    public class ProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }

        public ProductRequest()
        {
            
        }
    }
}
