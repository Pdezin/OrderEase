using Domain.DTOs.Roles;

namespace Domain.DTOs.Use{
    public class UserResultDetailDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public bool Active { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public RoleResultDTO Role { get; set; }
    }
}
