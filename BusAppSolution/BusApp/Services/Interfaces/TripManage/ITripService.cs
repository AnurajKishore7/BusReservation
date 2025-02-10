using BusApp.DTOs.TripManage;

namespace BusApp.Services.Interfaces.TripManage
{
    public interface ITripService
    {
        Task<IEnumerable<TripResponseDto>> GetAllTripsAsync();
        Task<TripResponseDto?> GetTripByIdAsync(int tripId);
        Task<TripResponseDto> AddTripAsync(TripAddDto tripDto);
        Task<TripResponseDto?> UpdateTripAsync(TripUpdateDto tripDto);
        Task<bool> DeleteTripAsync(int tripId);

        //For booking
        Task<IEnumerable<TripResponseDto>> GetAvailableTripsAsync(int routeId, DateTime? departureDate);

        Task<IEnumerable<TripResponseDto>> GetAvailableTripsByPriceAsync(int routeId, DateTime? departureDate, decimal? maxPrice);

    }
}
