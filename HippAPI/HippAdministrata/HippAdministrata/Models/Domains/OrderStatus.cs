using System.ComponentModel.DataAnnotations;

namespace HippAdministrata.Models.Domains
{
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }

    }
}
