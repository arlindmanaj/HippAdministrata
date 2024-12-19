namespace HippAdministrata.Models.DTOs
{
    public class SalesPersonClientsDto
    {
        public int Id { get; set; }
        public int SalesPersonId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string DeliveryDestination { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SalesPersonName { get; set; } // Simplify related data
        public string ClientName { get; set; }      // Simplify related data
    }
}
