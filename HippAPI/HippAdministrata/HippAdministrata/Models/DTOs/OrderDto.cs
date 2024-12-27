using HippAdministrata.Models.Enums;

namespace HippAdministrata.Models.DTOs
{
    public class OrderDto
    {
       
        public string? DeliveryDestination { get; set; } // The address for delivery

        public List<OrderProductDto> Products { get; set; } = new List<OrderProductDto>();
       
            public int Id { get; set; }
            public string ProductName { get; set; }
            public string ClientName { get; set; }
            public string SalesPersonName { get; set; }
            public decimal PricePercentageForEmployee { get; set; }
            public string Destination { get; set; } 
            public int Quantity { get; set; }
            public int UnlabeledQuantity { get; set; }
            public int LabeledQuantity { get; set; }
            public decimal ProductPrice { get; set; }
            public DateTime CreatedAt { get; set; }
            public string OrderStatus { get; set; }
            
        

    }
}
