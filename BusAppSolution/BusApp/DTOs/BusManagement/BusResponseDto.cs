namespace BusApp.DTOs.BusManagement
{
    public class BusResponseDto
    {
        public int Id { get; set; } 
        public string BusNo { get; set; } = string.Empty;
        public string BusType { get; set; } = string.Empty; 
        public int TotalSeats { get; set; }
        public int OperatorId { get; set; }
    }

}
