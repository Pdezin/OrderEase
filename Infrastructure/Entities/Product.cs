using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    [Table("products")]
    public class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
            ProductImages = new HashSet<ProductImage>();
            ProductPrices = new HashSet<ProductPrice>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Column("description")]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Column("weight")]
        public decimal Weight { get; set; }

        [Column("height")]
        public decimal Height { get; set; }

        [Column("width")]
        public decimal Width { get; set; }

        [Column("length")]
        public decimal Length { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("categories_id")]
        public int CategoryId { get; set; }


        public Category Category { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductPrice> ProductPrices { get; set; }
    }

    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Category)
                   .WithMany(e => e.Products)
                   .HasForeignKey(e => e.CategoryId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.OrderItems)
                   .WithOne(e => e.Product)
                   .HasForeignKey(e => e.ProductId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ProductImages)
                   .WithOne(e => e.Product)
                   .HasForeignKey(e => e.ProductId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ProductPrices)
                   .WithOne(e => e.Product)
                   .HasForeignKey(e => e.ProductId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
