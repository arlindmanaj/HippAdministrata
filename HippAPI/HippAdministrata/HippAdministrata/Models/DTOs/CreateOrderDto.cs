namespace HippAdministrata.Models.DTOs
{
    public class CreateOrderDto
    {
        public string DeliveryDestination { get; set; }
        public List<OrderProductDto> Products { get; set; } = new List<OrderProductDto>();
    }
}
