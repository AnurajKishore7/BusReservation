using BusApp.Models;
using BusApp.Repositories.Interfaces.BusRouteManage;
using BusReservationApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations.BusRouteManage
{
    public class BusRouteRepo : IBusRouteRepo
    {
        private readonly AppDbContext _context;

        public BusRouteRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BusRoute>> GetAllRoutesAsync()
        {
            try
            {
                var routes = await _context.BusRoutes.ToListAsync();
                return routes;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving bus routes.", ex);
            }
        }

        public async Task<BusRoute?> GetRouteByIdAsync(int routeId)
        {
            try
            {
                var route = await _context.BusRoutes.FindAsync(routeId);
                if (route == null)
                    throw new KeyNotFoundException($"Bus route with ID {routeId} not found.");

                return route;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the bus route with ID {routeId}.", ex);
            }
        }

        public async Task<BusRoute> AddRouteAsync(BusRoute route)
        {
            try
            {
                _context.BusRoutes.Add(route);
                await _context.SaveChangesAsync();
                return route;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the bus route.", ex);
            }
        }

        public async Task<BusRoute> UpdateRouteAsync(BusRoute route)
        {
            var existingRoute = await _context.BusRoutes.FindAsync(route.Id);
            if (existingRoute == null)
                throw new KeyNotFoundException($"Bus route with ID {route.Id} not found.");

            existingRoute.Source = route.Source;
            existingRoute.Destination = route.Destination;
            existingRoute.EstimatedDuration = route.EstimatedDuration;
            existingRoute.Distance = route.Distance;

            try
            {
                await _context.SaveChangesAsync();
                return route;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the bus route with ID {route.Id}.", ex);
            }
        }

        public async Task<bool> DeleteRouteAsync(int routeId)
        {
            var route = await _context.BusRoutes.FindAsync(routeId);
            if (route == null)
                throw new KeyNotFoundException($"Bus route with ID {routeId} not found.");

            _context.BusRoutes.Remove(route);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the bus route with ID {routeId}.", ex);
            }
        }
    }
}
