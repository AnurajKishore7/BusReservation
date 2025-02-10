using BusApp.DTOs.ClientManage;

namespace BusApp.Services.Interfaces.ClientManage
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllClientsAsync();
        Task<ClientDto?> GetClientByIdAsync(int clientId);
        Task<ClientDto?> GetClientByEmailAsync(string email);
        Task<ClientDto?> UpdateClientAsync(int clientId, UpdateClientDto clientDto);
        Task<bool> DeleteClientAsync(int clientId);
    }
}
