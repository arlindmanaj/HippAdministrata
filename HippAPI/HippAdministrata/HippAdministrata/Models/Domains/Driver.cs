namespace HippAdministrata.Models.Domains
{
    public class Driver
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? LicensePlate { get; set; }
        public string? CarModel { get; set; }
        public int UserId { get; set; } // FK to User
        public User? User { get; set; }

        public int OrderId {get; set; }

        public Order? Order { get; set; }

    }
}
