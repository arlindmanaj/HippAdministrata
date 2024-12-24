namespace HippAdministrata.Models.DTOs
{
    public class OrderDto
    {
        public int ProductId { get; set; } // The product being ordered
        public string? DeliveryDestination { get; set; } // The address for delivery
        public int Quantity { get; set; } // Quantity of the product
       
    }
}
