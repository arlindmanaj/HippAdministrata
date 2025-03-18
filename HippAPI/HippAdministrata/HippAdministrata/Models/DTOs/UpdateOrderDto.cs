public class UpdateOrderDto
{
    public int ProductId { get; set; } // Product ID (Foreign Key to Product)
    public string DeliveryDestination { get; set; } // New Delivery Address
    public int Quantity { get; set; } // New Quantity

}
