using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    [Table("roles")]
    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column("order_access")]
        public AccessType OrderAccess { get; set; }

        [Column("product_access")]
        public AccessType ProductAccess { get; set; }

        [Column("user_access")]
        public AccessType UserAccess { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }


        public ICollection<User> Users { get; set; }
    }

    public enum AccessType
    {
        Read = 1,
        Write = 2,
        None = 3
    }

    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(e => e.Users)
                   .WithOne(e => e.Role)
                   .HasForeignKey(e => e.RoleId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
