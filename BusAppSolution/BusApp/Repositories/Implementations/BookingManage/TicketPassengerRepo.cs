using BusApp.Models;
using BusApp.Repositories.Interfaces.BookingManage;
using BusReservationApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations.BookingManage
{
    public class TicketPassengerRepo : ITicketPassengerRepo
    {
        private readonly AppDbContext _context;

        public TicketPassengerRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTicketPassengersAsync(IEnumerable<TicketPassenger> ticketPassengers)
        {
            if (ticketPassengers == null || !ticketPassengers.Any())
                throw new ArgumentException("TicketPassengers list cannot be null or empty.");

            try
            {
                await _context.TicketPassengers.AddRangeAsync(ticketPassengers);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding ticket passengers.", ex);
            }
        }

        public async Task<IEnumerable<TicketPassenger>> GetTicketPassengersByBookingIdAsync(int bookingId)
        {
            if (bookingId <= 0)
                throw new ArgumentException("Invalid Booking ID.");

            try
            {
                var passengers = await _context.TicketPassengers
                    .Where(tp => tp.BookingId == bookingId)
                    .ToListAsync();

                if (passengers == null || !passengers.Any())
                    throw new Exception("No ticket passengers found for the given booking ID.");

                return passengers;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving ticket passengers.", ex);
            }
        }
    }
}
