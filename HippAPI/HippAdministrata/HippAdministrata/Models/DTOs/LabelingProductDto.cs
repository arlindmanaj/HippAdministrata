namespace HippAdministrata.Models.DTOs
{
    public class LabelingProductDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal LabelingQuantity { get; set; } // Quantity to label in this step
    }
}
