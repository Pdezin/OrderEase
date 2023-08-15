using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    [Table("customers")]
    public class Customer
    {
        public Customer()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            Orders = new HashSet<Order>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Column("email")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Column("fone")]
        [MaxLength(20)]
        public string Fone { get; set; }

        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }


        public ICollection<CustomerAddress> CustomerAddresses { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(e => e.CustomerAddresses)
                   .WithOne(e => e.Customer)
                   .HasForeignKey(e => e.CustomerId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Orders)
                   .WithOne(e => e.Customer)
                   .HasForeignKey(e => e.CustomerId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
