using BusApp.Models;

namespace BusApp.Repositories.Interfaces.BusRouteManage
{
    public interface IBusRouteRepo
    {
        Task<IEnumerable<BusRoute>> GetAllRoutesAsync();
        Task<BusRoute?> GetRouteByIdAsync(int routeId);
        Task<BusRoute> AddRouteAsync(BusRoute route);
        Task<BusRoute> UpdateRouteAsync(BusRoute route);
        Task<bool> DeleteRouteAsync(int routeId);
    }
}
