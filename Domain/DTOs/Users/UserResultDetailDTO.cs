namespace Domain.DTOs.Use{
    public class UserResultDetailDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public bool Active { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
