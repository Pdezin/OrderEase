﻿using Domain.DTOs.Roles;

namespace Domain.DTOs.Users
{
    public class UserResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public RoleResultDTO Role { get; set; }
    }
}
