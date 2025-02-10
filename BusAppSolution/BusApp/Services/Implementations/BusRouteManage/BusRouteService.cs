using BusApp.DTOs.BusRouteManage;
using BusApp.Models;
using BusApp.Repositories.Interfaces.BusRouteManage;
using BusApp.Services.Interfaces.BusRouteManage;

namespace BusApp.Services.Implementations.BusRouteManage
{
    public class BusRouteService : IBusRouteService
    {
        private readonly IBusRouteRepo _busRouteRepo;

        public BusRouteService(IBusRouteRepo busRouteRepo)
        {
            _busRouteRepo = busRouteRepo;
        }

        public async Task<IEnumerable<BusRouteResponseDto>> GetAllRoutesAsync()
        {
            try
            {
                var routes = await _busRouteRepo.GetAllRoutesAsync();
                return routes.Select(route => new BusRouteResponseDto
                {
                    Id = route.Id,
                    Source = route.Source,
                    Destination = route.Destination,
                    EstimatedDuration = route.EstimatedDuration,
                    Distance = route.Distance
                });
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while fetching routes.");
            }
        }

        public async Task<BusRouteResponseDto?> GetRouteByIdAsync(int routeId)
        {
            try
            {
                var route = await _busRouteRepo.GetRouteByIdAsync(routeId);
                if (route == null)
                    return null;

                return new BusRouteResponseDto
                {
                    Id = route.Id,
                    Source = route.Source,
                    Destination = route.Destination,
                    EstimatedDuration = route.EstimatedDuration,
                    Distance = route.Distance
                };
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while fetching route with ID {routeId}.");
            }
        }

        public async Task<BusRouteResponseDto> AddRouteAsync(BusRouteAddDto routeDto)
        {
            try
            {
                var route = new BusRoute
                {
                    Source = routeDto.Source,
                    Destination = routeDto.Destination,
                    EstimatedDuration = routeDto.EstimatedDuration,
                    Distance = routeDto.Distance
                };

                var createdRoute = await _busRouteRepo.AddRouteAsync(route);

                return new BusRouteResponseDto
                {
                    Id = createdRoute.Id,
                    Source = createdRoute.Source,
                    Destination = createdRoute.Destination,
                    EstimatedDuration = createdRoute.EstimatedDuration,
                    Distance = createdRoute.Distance
                };
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while adding the route.");
            }
        }

        public async Task<BusRouteResponseDto?> UpdateRouteAsync(BusRouteUpdateDto routeDto)
        {
            try
            {
                var existingRoute = await _busRouteRepo.GetRouteByIdAsync(routeDto.Id);
                if (existingRoute == null)
                    return null;

                existingRoute.Source = routeDto.Source;
                existingRoute.Destination = routeDto.Destination;
                existingRoute.EstimatedDuration = routeDto.EstimatedDuration;
                existingRoute.Distance = routeDto.Distance;

                var updatedRoute = await _busRouteRepo.UpdateRouteAsync(existingRoute);

                if (updatedRoute == null) return null;

                return new BusRouteResponseDto
                {
                    Id = updatedRoute.Id,
                    Source = updatedRoute.Source,
                    Destination = updatedRoute.Destination,
                    EstimatedDuration = updatedRoute.EstimatedDuration,
                    Distance = updatedRoute.Distance
                };
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while updating route with ID {routeDto.Id}.");
            }
        }

        public async Task<bool> DeleteRouteAsync(int routeId)
        {
            try
            {
                return await _busRouteRepo.DeleteRouteAsync(routeId);
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while deleting route with ID {routeId}.");
            }
        }
    }
}
