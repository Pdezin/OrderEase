using Infrastructure.Entities;

namespace Domain.DTOs.Roles
{
    public class RoleResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AccessType OrderAccess { get; set; }
        public AccessType ProductAccess { get; set; }
        public AccessType UserAccess { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
