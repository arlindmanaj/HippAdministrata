    using HippAdministrata.Models.Enums;

    namespace HippAdministrata.Models.Domains
    {
        public class Warehouse
        {
            public int Id { get; set; }
            public Location Location { get; set; } // Enum                                 
            public ICollection<Order>? Orders { get; set; } // Orders stored in this warehouse !!!!!!!!!!This neeeds fix when trying to update the warehouse on swagger it asks for orders 
        }
    }
