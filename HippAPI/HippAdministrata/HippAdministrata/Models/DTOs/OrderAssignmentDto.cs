namespace HippAdministrata.Models.DTOs
{
    public class OrderAssignmentDto
    {
        public int EmployeeId { get; set; } // ID of the assigned employee
        public int DriverId { get; set; } // ID of the assigned driver
        public int? WarehouseId { get; set; } // Optional: ID of the assigned warehouse
    }
}
