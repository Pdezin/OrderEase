using Domain.DTOs.Categories;
using Domain.DTOs.ProductPrices;

namespace Domain.DTOs.Products
{
    public class ProductResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal Stock { get; set; }
        public bool Active { get; set; }
        public CategoryResultDTO Category { get; set; }
        public List<ProductPricesResultDTO> ProductPrices { get; set; }
    }
}
