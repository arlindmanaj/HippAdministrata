using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;

namespace HippAdministrata.Models.JunctionTables
{
    public class OrderHistory
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public OrderStatus OldStatus { get; set; }
        public OrderStatus NewStatus { get; set; }
        public DateTime Timestamp { get; set; }
        public int UpdatedByEmployeeId { get; set; }
        public Employee? UpdatedByEmployee { get; set; }

    }
}
