using BusApp.Models;
using BusApp.Repositories.Interfaces.BookingManage;
using BusReservationApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations.BookingManage
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly AppDbContext _context;

        public PaymentRepo(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment), "Payment details cannot be null.");

            try
            {
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
                return payment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the payment.", ex);
            }
        }

        public async Task<Payment?> GetPaymentByBookingIdAsync(int bookingId)
        {
            if (bookingId <= 0)
                throw new ArgumentException("Invalid Booking ID.");

            try
            {
                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.BookingId == bookingId);

                if (payment == null)
                    throw new Exception("No payment record found for the given booking ID.");

                return payment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving payment details.", ex);
            }
        }
    }
}

