using HippAdministrata.Models.Domains;

namespace HippAdministrata.Models.JunctionTables
{
    public class SalesPersonClients
    {
        public int Id { get; set; }
        public int SalesPersonId {  get; set; }
        public int ClientId { get; set; }
        public string? Name { get; set; } // Order name
        
        public string? DeliveryDestination { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SalesPersonClientProduct>? Products { get; set; }
        public SalesPerson? SalesPerson { get; set; }
        public Client? Client { get; set; }
    }
}
