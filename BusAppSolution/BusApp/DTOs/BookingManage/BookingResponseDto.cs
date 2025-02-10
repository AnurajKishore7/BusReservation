namespace BusApp.DTOs.BookingManage
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public int ClientId { get; set; }
        public int TripId { get; set; }
        public DateTime BookedAt { get; set; }
        public string Status { get; set; }

        public List<TicketPassengerResponseDto> Passengers { get; set; } = new List<TicketPassengerResponseDto>();

        public PaymentResponseDto Payment { get; set; }
    }

}
