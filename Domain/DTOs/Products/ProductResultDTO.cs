namespace Domain.DTOs.Products
{
    public class ProductResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal Stock { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public bool Active { get; set; }
    }
}
