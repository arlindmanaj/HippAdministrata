using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;
using HippAdministrata.Models.JunctionTables;
namespace HippAdministrata.Models.Domains
{
    public class Order
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public string? DeliveryDestination { get; set; }
        public DateTime LastUpdated { get; set; }
        public int ClientId { get; set; }
        public int SalesPersonId { get; set; }
        public int EmployeeId {get; set;}
        public int DriverId { get; set; }
        public int? WarehouseId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public int OrderHistoryId { get; set; }
        public OrderHistory? OrderHistory { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public Client? Client { get; set; }
        public SalesPerson? SalesPerson { get; set; }
        public Employee? Employee { get; set; }
        public Warehouse? Warehouse { get; set; }
        public Driver? Driver { get; set; }

        public ICollection<OrderProduct>? OrderProducts { get; set; }

    }
}
