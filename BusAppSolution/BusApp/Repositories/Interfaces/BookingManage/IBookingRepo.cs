using BusApp.Models;

namespace BusApp.Repositories.Interfaces.BookingManage
{
    public interface IBookingRepo
    {
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByClientIdAsync(int clientId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<IEnumerable<Booking>> GetConfirmedBookingsAsync();
        Task<IEnumerable<Booking>> GetConfirmedBookingsByClientIdAsync(int clientId);
        Task<Booking?> UpdateBookingStatusAsync(int bookingId, string status);
        Task<bool> CancelBookingAsync(int bookingId);
    }
}
