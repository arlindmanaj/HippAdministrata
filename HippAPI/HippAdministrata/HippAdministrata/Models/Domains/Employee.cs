namespace HippAdministrata.Models.Domains
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public Manager? Supervisor { get; set; }
        public int UserId { get; set; } // FK to User
        public User? User { get; set; }

    }
}
