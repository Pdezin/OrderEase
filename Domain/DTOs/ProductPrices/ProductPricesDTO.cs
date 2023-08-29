namespace Domain.DTOs.ProductPrices
{
    public class ProductPricesDTO
    {
        public decimal Price { get; set; }
        public int PriceListId { get; set; }
        public bool? Remove { get; set; }
    }
}
