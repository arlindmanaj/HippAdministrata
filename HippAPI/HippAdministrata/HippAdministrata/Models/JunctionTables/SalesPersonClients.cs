using HippAdministrata.Models.Domains;

namespace HippAdministrata.Models.JunctionTables
{
    public class SalesPersonClients
    {
        public int Id { get; set; }
        public int SalesPersonId {  get; set; }
        public int ClientId { get; set; }
        public SalesPerson? SalesPerson { get; set; }
        public Client? Client { get; set; }
    }
}
