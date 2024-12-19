namespace HippAdministrata.Models.Domains
{
    public class Manager
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public int UserId { get; set; } // FK to User
        public User? User { get; set; }


    }
}
