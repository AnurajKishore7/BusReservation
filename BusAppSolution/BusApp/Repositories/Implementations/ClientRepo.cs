using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusReservationApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations
{
    public class ClientRepo : Repository<Client, int>, IClientRepo
    {
        public ClientRepo(AppDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the user by email.", ex);
            }
        }
    }
}
