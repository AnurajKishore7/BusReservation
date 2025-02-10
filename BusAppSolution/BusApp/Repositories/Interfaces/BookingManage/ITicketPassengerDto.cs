using BusApp.Models;

namespace BusApp.Repositories.Interfaces.BookingManage
{
    public interface ITicketPassengerRepo
    {
        Task AddTicketPassengersAsync(IEnumerable<TicketPassenger> ticketPassengers);
        Task<IEnumerable<TicketPassenger>> GetTicketPassengersByBookingIdAsync(int bookingId);
    }

}
