    using HippAdministrata.Models.Enums;

    namespace HippAdministrata.Models.Domains
    {
        public class Warehouse
        {
            public int Id { get; set; }
            public string Name { get; set; } 
            public Location Location { get; set; } // Enum                                 
           
        }
    }
