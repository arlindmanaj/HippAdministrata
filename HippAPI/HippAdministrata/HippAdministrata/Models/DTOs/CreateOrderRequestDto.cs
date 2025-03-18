//namespace HippAdministrata.Models.Dtos
//{
//    public class CreateOrderRequestDto
//    {
//        public int OrderId { get; set; }
//        public int ClientId { get; set; }
//        public string RequestType { get; set; } = string.Empty; // "Update" or "Delete"
//        public string Reason { get; set; } = string.Empty;
//    }
//}
namespace HippAdministrata.Models.Dtos
{
    public class CreateOrderRequestDto
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public string RequestType { get; set; } = string.Empty; // "Update" or "Delete"
        public string Reason { get; set; } = string.Empty;

        // Fields for update requests (optional)
        public string? NewDeliveryDestination { get; set; }
        public int? NewQuantity { get; set; }
        public int? NewProductId { get; set; }
        public int? UnlabeledQuantity { get; set; }
    }
}