using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    [Table("orders")]
    public class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("status")]
        public Status Status { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("freight")]
        public decimal Freight { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("users_id")]
        public int UserId { get; set; }

        [Column("customers_id")]
        public int CustomerId { get; set; }

        [Column("customer_addresses_id")]
        public int CustomerAddressId { get; set; }


        public User User { get; set; }
        public Customer Customer { get; set; }
        public CustomerAddress CustomerAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public enum Status
    {
        Open = 1,
        Confirmed = 2,
        Canceled = 3,
        Completed = 4
    }

    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.User)
                   .WithMany(e => e.Orders)
                   .HasForeignKey(e => e.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Customer)
                   .WithMany(e => e.Orders)
                   .HasForeignKey(e => e.CustomerId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CustomerAddress)
                   .WithMany(e => e.Orders)
                   .HasForeignKey(e => e.CustomerAddressId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.OrderItems)
                   .WithOne(e => e.Order)
                   .HasForeignKey(e => e.OrderId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
