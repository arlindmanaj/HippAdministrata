using HippAdministrata.Models.Enums;
using HippAdministrata.Models.JunctionTables;

namespace HippAdministrata.Models.Domains
{
    public class SalesPerson
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public Location Location { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<SalesPersonClients>? SalesPersonsClients { get; set; }
        public int UserId { get; set; } // FK to User
        public User? User { get; set; }
    }
}
