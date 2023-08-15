using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    [Table("categories")]
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }


        public ICollection<Product> Products { get; set; }
    }

    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(e => e.Products)
                   .WithOne(e => e.Category)
                   .HasForeignKey(e => e.CategoryId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
