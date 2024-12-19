namespace HippAdministrata.Models.Requests
{
    public class DriverUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string LicensePlate { get; set; }
        public string CarModel { get; set; }
    }
}
