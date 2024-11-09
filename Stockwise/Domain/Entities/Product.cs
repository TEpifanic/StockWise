namespace StockWise.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set;}
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set;}


    }
}
