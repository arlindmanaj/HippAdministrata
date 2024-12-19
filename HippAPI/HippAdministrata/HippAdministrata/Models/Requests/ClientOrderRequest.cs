using HippAdministrata.Models.Domains;

namespace HippAdministrata.Models.Requests
{
    public class ClientOrderRequest
    {

        public int Id { get; set; }
        public int ClientId { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public string? DeliveryDestination { get; set; }
        public int SalesPersonId { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public Client? Client { get; set; }
        public SalesPerson? SalesPerson { get; set; }
    }
}
