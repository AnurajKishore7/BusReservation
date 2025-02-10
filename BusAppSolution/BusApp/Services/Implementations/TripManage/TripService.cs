using BusApp.DTOs.TripManage;
using BusApp.Models;
using BusApp.Repositories.Interfaces.TripManage;
using BusApp.Services.Interfaces.TripManage;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Services.Implementations.TripManage
{
    public class TripService : ITripService
    {
        private readonly ITripRepo _tripRepo;

        public TripService(ITripRepo tripRepo)
        {
            _tripRepo = tripRepo;
        }

        public async Task<IEnumerable<TripResponseDto>> GetAllTripsAsync()
        {
            try
            {
                var trips = await _tripRepo.GetAllTripsAsync();
                if (trips == null || !trips.Any())
                    throw new Exception("No trips found.");

                return trips.Select(t => new TripResponseDto
                {
                    Id = t.Id,
                    BusRouteId = t.BusRouteId,
                    BusId = t.BusId,
                    DepartureTime = t.DepartureTime,
                    ArrivalTime = t.ArrivalTime,
                    Price = t.Price
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving trips: {ex.Message}");
            }
        }

        public async Task<TripResponseDto?> GetTripByIdAsync(int tripId)
        {
            try
            {
                if (tripId <= 0)
                    throw new ArgumentException("Invalid trip ID.");

                var trip = await _tripRepo.GetTripByIdAsync(tripId);
                if (trip == null)
                    throw new KeyNotFoundException("Trip not found.");

                return new TripResponseDto
                {
                    Id = trip.Id,
                    BusRouteId = trip.BusRouteId,
                    BusId = trip.BusId,
                    DepartureTime = trip.DepartureTime,
                    ArrivalTime = trip.ArrivalTime,
                    Price = trip.Price
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the trip: {ex.Message}");
            }
        }

        public async Task<TripResponseDto> AddTripAsync(TripAddDto tripDto)
        {
            try
            {
                if (tripDto == null)
                    throw new ArgumentNullException(nameof(tripDto), "Trip data cannot be null.");

                if (tripDto.DepartureTime >= tripDto.ArrivalTime)
                    throw new ArgumentException("Arrival time must be after departure time.");

                var trip = new Trip
                {
                    BusRouteId = tripDto.BusRouteId,
                    BusId = tripDto.BusId,
                    DepartureTime = tripDto.DepartureTime,
                    ArrivalTime = tripDto.ArrivalTime,
                    Price = tripDto.Price
                };

                var createdTrip = await _tripRepo.AddTripAsync(trip);
                if (createdTrip == null)
                    throw new Exception("Failed to add trip.");

                return new TripResponseDto
                {
                    Id = createdTrip.Id,
                    BusRouteId = createdTrip.BusRouteId,
                    BusId = createdTrip.BusId,
                    DepartureTime = createdTrip.DepartureTime,
                    ArrivalTime = createdTrip.ArrivalTime,
                    Price = createdTrip.Price
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the trip: {ex.Message}");
            }
        }

        public async Task<TripResponseDto?> UpdateTripAsync(TripUpdateDto tripDto)
        {
            try
            {
                if (tripDto == null)
                    throw new ArgumentNullException(nameof(tripDto), "Trip data cannot be null.");

                var existingTrip = await _tripRepo.GetTripByIdAsync(tripDto.Id);
                if (existingTrip == null)
                    throw new KeyNotFoundException("Trip not found.");

                if (tripDto.DepartureTime >= tripDto.ArrivalTime)
                    throw new ArgumentException("Arrival time must be after departure time.");

                // Updating trip details
                existingTrip.BusRouteId = tripDto.BusRouteId;
                existingTrip.BusId = tripDto.BusId;
                existingTrip.DepartureTime = tripDto.DepartureTime;
                existingTrip.ArrivalTime = tripDto.ArrivalTime;
                existingTrip.Price = tripDto.Price;

                var updatedTrip = await _tripRepo.UpdateTripAsync(existingTrip);
                if (updatedTrip == null)
                    throw new Exception("Failed to update trip.");

                return new TripResponseDto
                {
                    Id = updatedTrip.Id,
                    BusRouteId = updatedTrip.BusRouteId,
                    BusId = updatedTrip.BusId,
                    DepartureTime = updatedTrip.DepartureTime,
                    ArrivalTime = updatedTrip.ArrivalTime,
                    Price = updatedTrip.Price
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the trip: {ex.Message}");
            }
        }

        public async Task<bool> DeleteTripAsync(int tripId)
        {
            try
            {
                if (tripId <= 0)
                    throw new ArgumentException("Invalid trip ID.");

                var tripExists = await _tripRepo.GetTripByIdAsync(tripId);
                if (tripExists == null)
                    throw new KeyNotFoundException("Trip not found.");

                var result = await _tripRepo.DeleteTripAsync(tripId);
                if (!result)
                    throw new Exception("Failed to delete trip.");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the trip: {ex.Message}");
            }
        }

        //For Booking
        public async Task<IEnumerable<TripResponseDto>> GetAvailableTripsAsync(int routeId, DateTime? departureDate)
        {
            try
            {
                if (routeId <= 0)
                    throw new ArgumentException("Invalid Route ID.");

                var trips = await _tripRepo.GetAvailableTripsAsync(routeId, departureDate);
                if (trips == null || !trips.Any())
                    throw new Exception("No available trips found for the given criteria.");

                return trips.Select(t => new TripResponseDto
                {
                    Id = t.Id,
                    BusRouteId = t.BusRouteId,
                    BusId = t.BusId,
                    DepartureTime = t.DepartureTime,
                    ArrivalTime = t.ArrivalTime,
                    Price = t.Price
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving available trips.", ex);
            }
        }

        public async Task<IEnumerable<TripResponseDto>> GetAvailableTripsByPriceAsync(int routeId, DateTime? departureDate, decimal? maxPrice)
        {
            if (routeId <= 0)
            {
                throw new ArgumentException("Invalid Route ID.");
            }

            try
            {
                var trips = await _tripRepo.GetAvailableTripsByPriceAsync(routeId, departureDate, maxPrice);

                if (trips == null || !trips.Any())
                {
                    return new List<TripResponseDto>(); 
                }

                return trips.Select(t => new TripResponseDto
                {
                    Id = t.Id,
                    BusRouteId = t.BusRouteId,
                    BusId = t.BusId,
                    DepartureTime = t.DepartureTime,
                    ArrivalTime = t.ArrivalTime,
                    Price = t.Price
                });
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while retrieving available trips.");
            }
        }

    }
}
