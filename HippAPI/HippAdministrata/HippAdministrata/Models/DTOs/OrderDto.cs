namespace HippAdministrata.Models.DTOs
{
    public class OrderDto
    {
       
        public string? DeliveryDestination { get; set; } // The address for delivery

        public List<OrderProductDto> Products { get; set; } = new List<OrderProductDto>();
    }
}
