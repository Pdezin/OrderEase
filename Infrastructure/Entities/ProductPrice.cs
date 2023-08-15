using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    [Table("product_prices")]
    public class ProductPrice
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("price_lists_id")]
        public int PriceListId { get; set; }

        [Column("products_id")]
        public int ProductId { get; set; }


        public PriceList PriceList { get; set; }
        public Product Product { get; set; }
    }

    public class ProductPriceMap : IEntityTypeConfiguration<ProductPrice>
    {
        public void Configure(EntityTypeBuilder<ProductPrice> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.PriceList)
                   .WithMany(e => e.ProductPrices)
                   .HasForeignKey(e => e.PriceListId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Product)
                   .WithMany(e => e.ProductPrices)
                   .HasForeignKey(e => e.ProductId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
