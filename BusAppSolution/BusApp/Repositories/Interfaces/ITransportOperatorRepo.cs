using BusApp.Models;

namespace BusApp.Repositories.Interfaces
{
    public interface ITransportOperatorRepo : IRepository<TransportOperator, int>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
