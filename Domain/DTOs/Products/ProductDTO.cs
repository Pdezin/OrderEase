namespace Domain.DTOs.Products
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Stock { get; set; }
        public bool Active { get; set; }
        public int CategoryId { get; set; }
    }
}
