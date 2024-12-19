using HippAdministrata.Models.JunctionTables;
using System.ComponentModel.DataAnnotations;

namespace HippAdministrata.Models.Domains
{
    public class Product
    {
       
        public int Id { get; set; }
        public string? Name { get; set; }      
        public string? ImageUrl { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal UnlabeledQuantity { get; set; }
        public decimal LabeledQuantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<OrderProduct>? OrderProducts { get; set; }

        public ICollection<EmployeeProductLabel>? EmployeeProductLabels { get; set; }
    }
}
