using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    [Table("price_lists")]
    public class PriceList
    {
        public PriceList()
        {
            OrderItems = new HashSet<OrderItem>();
            ProductPrices = new HashSet<ProductPrice>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }


        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<ProductPrice> ProductPrices { get; set; }
    }

    public class PriceListMap : IEntityTypeConfiguration<PriceList>
    {
        public void Configure(EntityTypeBuilder<PriceList> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(e => e.ProductPrices)
                   .WithOne(e => e.PriceList)
                   .HasForeignKey(e => e.PriceListId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.OrderItems)
                   .WithOne(e => e.PriceList)
                   .HasForeignKey(e => e.PriceListId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
