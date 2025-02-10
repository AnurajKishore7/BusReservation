namespace BusApp.DTOs.BookingManage
{
    public class TicketPassengerResponseDto
    {
        public int Id { get; set; }  // Passenger ID
        public string Name { get; set; }
        public int SeatNo { get; set; }
        public string Contact { get; set; }
        public bool IsDisabled { get; set; }
    }

}
