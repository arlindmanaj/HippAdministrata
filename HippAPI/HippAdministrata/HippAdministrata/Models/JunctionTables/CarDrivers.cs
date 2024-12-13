using HippAdministrata.Models.Domains;

namespace HippAdministrata.Models.JunctionTables
{
    public class CarDrivers
    {
        public int DriverId { get; set; }
        public int CarId { get; set; }
        public Driver Driver { get; set; }

    }
}
