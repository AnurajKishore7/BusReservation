using BusApp.Models;

namespace BusApp.Repositories.Interfaces.ClientManage
{
    public interface IClientManageRepo
    {
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(int clientId);
        Task<Client?> GetClientByEmailAsync(string email);
        Task<Client?> UpdateClientAsync(Client client);
        Task<bool> DeleteClientAsync(int clientId);
    }
}
