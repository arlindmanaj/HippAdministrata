using HippAdministrata.Models.Enums;

namespace HippAdministrata.Models.Requests
{
    public class OrderCreateRequest
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string DeliveryDestination { get; set; }
        public int ClientId { get; set; }
        public int SalesPersonId { get; set; }
        public int EmployeeId { get; set; }
        public int DriverId { get; set; }
        public int? WarehouseId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
