using BusApp.DTOs.BookingManage;

namespace BusApp.Services.Interfaces.BookingManage
{
    public interface IBookingService
    {
        Task<BookingResponseDto> CreateBookingAsync(BookingRequestDto bookingRequest);
        Task<BookingResponseDto?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<BookingResponseDto>> GetBookingsByClientIdAsync(int clientId);
        Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync();
        Task<IEnumerable<BookingResponseDto>> GetConfirmedBookingsAsync();
        Task<IEnumerable<BookingResponseDto>> GetConfirmedBookingsByClientIdAsync(int clientId);
        Task<bool> CancelBookingAsync(int bookingId);
        Task<bool> UpdateBookingStatusAsync(int bookingId, string status);
    }

}
