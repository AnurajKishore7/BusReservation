using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusReservationApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations
{
    public class TransOpManageRepo : ITransOpManageRepo
    {
        private readonly AppDbContext _context;

        public TransOpManageRepo(AppDbContext dbcontext)
        {
            _context = dbcontext; 
        }

        public async Task<bool> AddBusAsync(Bus bus)
        {
            await _context.Buses.AddAsync(bus);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteBusAsync(int busId)
        {
            var bus = await _context.Buses.FindAsync(busId);
            if (bus == null) return false;

            _context.Buses.Remove(bus);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Bus?> GetBusByIdAsync(int busId)
        {
            return await _context.Buses.FindAsync(busId);
        }

        public async Task<IEnumerable<Bus>> GetBusesByOperatorIdAsync(int operatorId)
        {
            return await _context.Buses
                        .Where(b => b.OperatorId == operatorId)
                        .ToListAsync();
        }

        public async Task<bool> UpdateBusAsync(Bus bus)
        {
            _context.Buses.Update(bus);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
