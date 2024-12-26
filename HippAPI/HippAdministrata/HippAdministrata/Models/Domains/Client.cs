using HippAdministrata.Models.JunctionTables;

namespace HippAdministrata.Models.Domains
{
    public class Client 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        
        public ICollection<Order>? Orders { get; set; }
       
        public int UserId { get; set; } // FK to User
        public User? User { get; set; }
    }
}
