namespace Domain.DTOs.PriceLists
{
    public class PriceListsResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
