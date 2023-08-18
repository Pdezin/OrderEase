using Infrastructure.Entities;

namespace Domain.DTOs.Roles
{
    public class RoleDTO
    {
        public string Name { get; set; }
        public AccessType OrderAccess { get; set; }
        public AccessType ProductAccess { get; set; }
        public AccessType UserAccess { get; set; }
    }
}
