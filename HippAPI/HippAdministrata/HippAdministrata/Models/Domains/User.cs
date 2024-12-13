namespace HippAdministrata.Models.Domains
{
    public class User
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public Role? Role { get; set; }

    }
}
