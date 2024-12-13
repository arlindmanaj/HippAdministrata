using HippAdministrata.Models.Domains;

namespace HippAdministrata.Models.Requests
{
    public class RegisterDriverRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? LicensePlate { get; set; }
        public string? CarModel { get; set; }

    }
}
