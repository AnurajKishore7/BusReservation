namespace BusApp.DTOs.BusRouteManage
{
    public class BusRouteResponseDto
    {
        public int Id { get; set; }
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public string? EstimatedDuration { get; set; }
        public int Distance { get; set; }
    }
}
