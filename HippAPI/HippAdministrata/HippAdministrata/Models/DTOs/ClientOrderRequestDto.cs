namespace HippAdministrata.Models.DTOs
{
    public class ClientOrderRequestDto
    {
        public string? Name { get; set; }
        public List<ProductSelectionDto> Products { get; set; }
        public string? DeliveryDestination { get; set; }
    }
}
