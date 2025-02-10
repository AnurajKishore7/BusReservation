using BusApp.DTOs.BusRouteManage;

namespace BusApp.Services.Interfaces.BusRouteManage
{
    public interface IBusRouteService
    {
        Task<IEnumerable<BusRouteResponseDto>> GetAllRoutesAsync();
        Task<BusRouteResponseDto?> GetRouteByIdAsync(int routeId);
        Task<BusRouteResponseDto> AddRouteAsync(BusRouteAddDto routeDto);
        Task<BusRouteResponseDto> UpdateRouteAsync(BusRouteUpdateDto routeDto);
        Task<bool> DeleteRouteAsync(int routeId);
    }
}
