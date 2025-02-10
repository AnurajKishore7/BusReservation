namespace BusApp.DTOs.ClientManage
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateOnly? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Contact { get; set; }
        public bool IsDiabled { get; set; }
    }
}
