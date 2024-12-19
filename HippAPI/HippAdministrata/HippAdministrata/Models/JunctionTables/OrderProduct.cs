using HippAdministrata.Models.Domains;

namespace HippAdministrata.Models.JunctionTables
{
   
        public class OrderProduct
        {
            public int OrderId { get; set; }
            public Order? Order { get; set; }

            public int ProductId { get; set; }
            public Product? Product { get; set; }

            public int Quantity { get; set; } // Quantity of this product in the order
        }

    
}
