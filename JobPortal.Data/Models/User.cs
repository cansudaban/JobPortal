﻿namespace JobPortal.Data.Models
{
    public class User : BaseEntities
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
