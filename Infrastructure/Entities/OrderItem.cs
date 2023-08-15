using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    [Table("order_items")]
    public class OrderItem
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("quantity")]
        public decimal Quantity { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("discount")]
        public decimal Discount { get; set; }

        [Column("addition")]
        public decimal Addition { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("orders_id")]
        public int OrderId { get; set; }

        [Column("products_id")]
        public int ProductId { get; set; }

        [Column("price_lists_id")]
        public int PriceListId { get; set; }


        public Order Order { get; set; }
        public Product Product { get; set; }
        public PriceList PriceList { get; set; }
    }

    public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Order)
                   .WithMany(e => e.OrderItems)
                   .HasForeignKey(e => e.OrderId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Product)
                   .WithMany(e => e.OrderItems)
                   .HasForeignKey(e => e.ProductId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PriceList)
                   .WithMany(e => e.OrderItems)
                   .HasForeignKey(e => e.PriceListId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
