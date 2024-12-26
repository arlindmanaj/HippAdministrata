using HippAdministrata.Models.Domains;

namespace HippAdministrata.Models.Requests
{
    public class RegisterClientRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
