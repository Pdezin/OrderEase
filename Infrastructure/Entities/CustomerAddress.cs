using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    [Table("customer_addresses")]
    public class CustomerAddress
    {
        public CustomerAddress()
        {
            Orders = new HashSet<Order>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("zip_code")]
        [MaxLength(10)]
        public string ZipCode { get; set; }

        [Column("state")]
        [MaxLength(10)]
        public string State { get; set; }

        [Column("city")]
        [MaxLength(100)]
        public string City { get; set; }

        [Column("neighborhood")]
        [MaxLength(200)]
        public string Neighborhood { get; set; }

        [Column("street")]
        [MaxLength(200)]
        public string Street { get; set; }

        [Column("complement")]
        [MaxLength(200)]
        public string Complement { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("customers_id")]
        public int CustomerId { get; set; }


        public Customer Customer { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public class CustomerAddressMap : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Customer)
                   .WithMany(e => e.CustomerAddresses)
                   .HasForeignKey(e => e.CustomerId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Orders)
                   .WithOne(e => e.CustomerAddress)
                   .HasForeignKey(e => e.CustomerAddressId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
