using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    [Table("users")]
    public class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Column("password")]
        [MaxLength(20)]
        public string Password { get; set; }

        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Column("birth_date")]
        public DateTime? BirthDate { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("roles_id")]
        public int RoleId { get; set; }


        public Role Role { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Role)
                   .WithMany(e => e.Users)
                   .HasForeignKey(e => e.RoleId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Orders)
                   .WithOne(e => e.User)
                   .HasForeignKey(e => e.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
