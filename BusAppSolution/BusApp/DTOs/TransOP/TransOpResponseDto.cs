namespace BusApp.DTOs.TransOP
{
    public class TransOpResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Contact { get; set; } = string.Empty;
    }
}
