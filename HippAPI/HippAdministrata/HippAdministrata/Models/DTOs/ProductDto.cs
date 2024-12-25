namespace HippAdministrata.Models.DTOs
{
    public class ProductDto
    {
       
            public string? Name { get; set; }
            public decimal UnlabeledQuantity { get; set; }
            public decimal LabeledQuantity { get; set; }
            public decimal Price { get; set; }
            public decimal PricePercentageForEmployee { get; set; }
            public int WarehouseId { get; set; }
        

    }
}
