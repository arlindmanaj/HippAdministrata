using HippAdministrata.Models.Domains;

namespace HippAdministrata.Models.JunctionTables
{
    public class SalesPersonClientProduct
    {
        public int Id { get; set; }
        public int SalesPersonClientId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public SalesPersonClients? SalesPersonClient { get; set; }
        public Product? Product { get; set; }
    }
}
