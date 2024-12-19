using HippAdministrata.Models.Enums;

namespace HippAdministrata.Models.DTOs
{
    public class OrderProcessRequestDto
    {
        public int EmployeeId { get; set; }
        public int DriverId { get; set; } 
        public int WarehouseId { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Created; 
    }
}
