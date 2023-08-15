using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    [Table("product_images")]
    public class ProductImage
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column("archive")]
        [MaxLength(200)]
        public string Archive { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("products_id")]
        public int ProductId { get; set; }


        public Product Product { get; set; }
    }

    public class ProductImageMap : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Product)
                   .WithMany(e => e.ProductImages)
                   .HasForeignKey(e => e.ProductId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
