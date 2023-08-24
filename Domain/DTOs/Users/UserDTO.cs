namespace Domain.DTOs.Users
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public bool Active { get; set; }
        public int RoleId { get; set; }
    }
}
