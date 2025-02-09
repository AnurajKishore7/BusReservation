using BusApp.Models;

namespace BusApp.Repositories.Interfaces
{
    public interface IClientRepo : IRepository<Client, int>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
