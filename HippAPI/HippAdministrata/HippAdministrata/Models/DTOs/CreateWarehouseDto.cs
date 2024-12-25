using HippAdministrata.Models.Enums;

namespace HippAdministrata.Models.DTOs
{
    public class CreateWarehouseDto
    {
        public string? Name { get; set; }
        public Location Location { get; set; }
    }
}
