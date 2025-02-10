using BusApp.Models;
using BusApp.Repositories.Interfaces.BookingManage;
using BusReservationApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations.BookingManage
{
    public class BookingRepo : IBookingRepo
    {
        private readonly AppDbContext _context;

        public BookingRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking), "Booking cannot be null.");

            try
            {
                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();
                return booking;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the booking.", ex);
            }
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            if (bookingId <= 0)
                throw new ArgumentException("Invalid booking ID.", nameof(bookingId));

            try
            {
                return await _context.Bookings
                    .Include(b => b.Client)
                    .Include(b => b.Trip)
                    .Include(b => b.TicketPassengers)
                    .Include(b => b.Payment)
                    .FirstOrDefaultAsync(b => b.Id == bookingId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the booking by ID.", ex);
            }
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            try
            {
                return await _context.Bookings
                    .Include(b => b.Client)
                    .Include(b => b.Trip)
                    .Include(b => b.TicketPassengers)
                    .Include(b => b.Payment)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all bookings.", ex);
            }
        }

        public async Task<IEnumerable<Booking>> GetBookingsByClientIdAsync(int clientId)
        {
            if (clientId <= 0)
                throw new ArgumentException("Invalid client ID.", nameof(clientId));

            try
            {
                return await _context.Bookings
                    .Where(b => b.ClientId == clientId)
                    .Include(b => b.Trip)
                    .Include(b => b.TicketPassengers)
                    .Include(b => b.Payment)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching bookings for the client.", ex);
            }
        }

        public async Task<IEnumerable<Booking>> GetConfirmedBookingsAsync()
        {
            try
            {
                return await _context.Bookings
                    .Where(b => b.Status == "Confirmed")
                    .Include(b => b.Client)
                    .Include(b => b.Trip)
                    .Include(b => b.TicketPassengers)
                    .Include(b => b.Payment)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching confirmed bookings.", ex);
            }
        }

        public async Task<IEnumerable<Booking>> GetConfirmedBookingsByClientIdAsync(int clientId)
        {
            if (clientId <= 0)
                throw new ArgumentException("Invalid client ID.", nameof(clientId));

            try
            {
                return await _context.Bookings
                    .Where(b => b.ClientId == clientId && b.Status == "Confirmed")
                    .Include(b => b.Trip)
                    .Include(b => b.TicketPassengers)
                    .Include(b => b.Payment)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching confirmed bookings for the client.", ex);
            }
        }

        public async Task<Booking?> UpdateBookingStatusAsync(int bookingId, string status)
        {
            if (bookingId <= 0)
                throw new ArgumentException("Invalid booking ID.", nameof(bookingId));

            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be empty.", nameof(status));

            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                    return null;

                booking.Status = status;
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();
                return booking;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the booking status.", ex);
            }
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            if (bookingId <= 0)
                throw new ArgumentException("Invalid booking ID.", nameof(bookingId));

            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                    return false;

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while canceling the booking.", ex);
            }
        }
    }
}
