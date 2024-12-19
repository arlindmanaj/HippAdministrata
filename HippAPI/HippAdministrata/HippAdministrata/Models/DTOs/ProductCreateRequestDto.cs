namespace HippAdministrata.Models.DTOs
{
    public class ProductCreateRequestDto
    {
        public string Name { get; set; } // Product name
        public decimal UnlabeledQuantity { get; set; } // Initial stock
    }
}
