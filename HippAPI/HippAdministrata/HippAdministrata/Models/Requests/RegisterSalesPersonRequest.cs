using HippAdministrata.Models.Enums;

namespace HippAdministrata.Models.Requests
{
    public class RegisterSalesPersonRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public Location Location { get; set; }
    }

}
