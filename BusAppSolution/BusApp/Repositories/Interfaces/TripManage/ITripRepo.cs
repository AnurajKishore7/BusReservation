using BusApp.Models;

namespace BusApp.Repositories.Interfaces.TripManage
{
        public interface ITripRepo
        {
            Task<IEnumerable<Trip>> GetAllTripsAsync();
            Task<Trip?> GetTripByIdAsync(int tripId);
            Task<Trip> AddTripAsync(Trip trip);
            Task<Trip?> UpdateTripAsync(Trip trip);
            Task<bool> DeleteTripAsync(int tripId);

        //For booking
        Task<IEnumerable<Trip>> GetAvailableTripsAsync(int routeId, DateTime? departureDate);

        Task<IEnumerable<Trip>> GetAvailableTripsByPriceAsync(int routeId, DateTime? departureDate, decimal? maxPrice);
    }
}
