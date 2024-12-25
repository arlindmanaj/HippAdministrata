using HippAdministrata.Models.Enums;

namespace HippAdministrata.Models.Requests
{
    public class RegisterSalesPersonRequest
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
       
        public string Email { get; set; }
    }

}
