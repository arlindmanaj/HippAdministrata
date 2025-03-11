namespace HippAdministrata.Models.Dtos
{
    public class RejectRequestDto
    {
        public int RequestId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
