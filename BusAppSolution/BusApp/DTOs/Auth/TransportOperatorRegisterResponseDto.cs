namespace BusApp.DTOs.Auth
{
    public class TransportOperatorRegisterResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
    }
}
