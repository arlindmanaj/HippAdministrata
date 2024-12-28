using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;
using HippAdministrata.Models.JunctionTables;
namespace HippAdministrata.Models.Domains
{
    public class Order
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key references
        public int ProductId { get; set; } // Foreign Key to Product
        public int ClientId { get; set; } // Foreign Key to Client
        public int SalesPersonId { get; set; } // Foreign Key to SalesPerson

        // Order Details
        public string? DeliveryDestination { get; set; } // Delivery Address
        public int Quantity { get; set; }
        public int UnlabeledQuantity { get; set; } // Specific to this order
        public int LabeledQuantity { get; set; } // Specific to this order
        public decimal ProductPrice { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdated { get; set; }

        // Order Workflow Status
        public int? OrderStatusId { get; set; }

        // Post-assignment nullable properties
        public int? EmployeeId { get; set; } // Assigned Employee
        public int? DriverId { get; set; } // Assigned Driver
        public int? WarehouseId { get; set; } // Assigned Warehouse

        // Navigation Properties (optional, useful for EF Core relationships)
        public Product? Product { get; set; }
        public Client? Client { get; set; }
        public SalesPerson? SalesPerson { get; set; }
        public Employee? Employee { get; set; }
        public Driver? Driver { get; set; }
        public Warehouse? Warehouse { get; set; }
        public OrderStatus? OrderStatus { get; set; }

    }

}
