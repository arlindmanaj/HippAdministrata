using System.ComponentModel.DataAnnotations;

namespace HippAdministrata.Models.Domains
{
    public class Product
    {
       
        public int Id { get; set; }
        public string? Name { get; set; }      
        public decimal UnlabeledQuantity { get; set; }
        public decimal LabeledQuantity { get; set; }
        public decimal Price { get; set; } // Price per product for the client
        public decimal PricePercentageForEmployee { get; set; } // Employee's pay percentage per label
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
