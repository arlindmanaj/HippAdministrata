using System.ComponentModel.DataAnnotations;

namespace HippAdministrata.Models.Domains
{
    public class Product
    {
       
        public int Id { get; set; }
        public string? Name { get; set; }      
        public decimal UnlabeledQuantity { get; set; }
        public decimal LabeledQuantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
