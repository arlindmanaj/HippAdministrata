using HippAdministrata.Models.Domains;

namespace HippAdministrata.Models.JunctionTables
{
    public class EmployeeProductLabel
    {
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int LabeledQuantity { get; set; }
    }
}
