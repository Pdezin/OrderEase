using Domain.DTOs.Categories;
using Domain.DTOs.ProductPrices;

namespace Domain.DTOs.Products
{
    public class ProductResultDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Stock { get; set; }
        public bool Active { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public CategoryResultDTO Category { get; set; }
        public List<ProductPricesResultDTO> ProductPrices { get; set; }
    }
}
