namespace BusApp.DTOs.BookingManage
{
    public class BookingRequestDto
    {
        public int ClientId { get; set; }
        public int TripId { get; set; } 

        public List<TicketPassengerDto> Passengers { get; set; } = new List<TicketPassengerDto>();

        public PaymentRequestDto Payment { get; set; } = new PaymentRequestDto();
    }

}
