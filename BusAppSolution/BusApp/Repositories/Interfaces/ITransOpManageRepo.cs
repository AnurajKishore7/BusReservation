using BusApp.Models;

namespace BusApp.Repositories.Interfaces
{
    public interface ITransOpManageRepo
    {
        Task<bool> AddBusAsync(Bus bus);
        Task<IEnumerable<Bus>> GetBusesByOperatorIdAsync(int operatorId);
        Task<Bus?> GetBusByIdAsync(int busId);
        Task<bool> UpdateBusAsync(Bus bus);
        Task<bool> DeleteBusAsync(int busId);
    }
}
