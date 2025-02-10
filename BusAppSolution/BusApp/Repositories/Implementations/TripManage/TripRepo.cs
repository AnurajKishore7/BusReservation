using BusApp.Models;
using BusApp.Repositories.Interfaces.TripManage;
using BusReservationApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations.TripManage
{
    public class TripRepo : ITripRepo
    {
        private readonly AppDbContext _context;

        public TripRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trip>> GetAllTripsAsync()
        {
            try
            {
                var trips = await _context.Trips
                    .Include(t => t.BusRoute)
                    .Include(t => t.Bus)
                    .ToListAsync();

                if (trips == null || !trips.Any())
                {
                    throw new KeyNotFoundException("No trips found.");
                }

                return trips;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("No trips available.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching trips.", ex);
            }
        }

        public async Task<Trip?> GetTripByIdAsync(int tripId)
        {
            try
            {
                var trip = await _context.Trips
                    .Include(t => t.BusRoute)
                    .Include(t => t.Bus)
                    .FirstOrDefaultAsync(t => t.Id == tripId);

                if (trip == null)
                {
                    throw new KeyNotFoundException($"Trip with ID {tripId} not found.");
                }

                return trip;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Trip with ID {tripId} does not exist.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching trip with ID {tripId}.", ex);
            }
        }

        public async Task<Trip> AddTripAsync(Trip trip)
        {
            try
            {
                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();
                return trip;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the trip.", ex);
            }
        }

        public async Task<Trip?> UpdateTripAsync(Trip trip)
        {
            var existingTrip = await _context.Trips.FindAsync(trip.Id);
            if (existingTrip == null)
                return null;

            try
            {
                existingTrip.BusRouteId = trip.BusRouteId;
                existingTrip.BusId = trip.BusId;
                existingTrip.DepartureTime = trip.DepartureTime;
                existingTrip.ArrivalTime = trip.ArrivalTime;
                existingTrip.Price = trip.Price;

                await _context.SaveChangesAsync();
                return existingTrip;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating trip with ID {trip.Id}.", ex);
            }
        }

        public async Task<bool> DeleteTripAsync(int tripId)
        {
            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null)
                return false;

            try
            {
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting trip with ID {tripId}.", ex);
            }
        }

    }
}
