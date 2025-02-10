namespace BusApp.DTOs.TripManage
{
    public class TripResponseDto
    {
        public int Id { get; set; }
        public int BusRouteId { get; set; }
        public int BusId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
    }
}
