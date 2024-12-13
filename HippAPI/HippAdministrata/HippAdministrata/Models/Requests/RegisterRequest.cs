using HippAdministrata.Models.Domains;

namespace HippAdministrata.Models.Requests
{
    public class RegisterRequest
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; } 
    }
}
