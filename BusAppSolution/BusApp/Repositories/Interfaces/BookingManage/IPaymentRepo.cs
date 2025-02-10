using BusApp.Models;

namespace BusApp.Repositories.Interfaces.BookingManage
{
    public interface IPaymentRepo
    {
        Task<Payment> AddPaymentAsync(Payment payment);
        Task<Payment?> GetPaymentByBookingIdAsync(int bookingId);
    }

}
