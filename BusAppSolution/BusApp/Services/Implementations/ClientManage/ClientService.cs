using BusApp.DTOs.ClientManage;
using BusApp.Repositories.Interfaces.ClientManage;
using BusApp.Services.Interfaces.ClientManage;

namespace BusApp.Services.Implementations.ClientManage
{
    public class ClientService : IClientService
    {
        private readonly IClientManageRepo _clientRepo;

        public ClientService(IClientManageRepo clientRepo)
        {
            _clientRepo = clientRepo;
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
        {
            try
            {
                var clients = await _clientRepo.GetAllClientsAsync();
                if (clients == null || !clients.Any())
                    return Enumerable.Empty<ClientDto>();

                return clients.Select(client => new ClientDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    DOB = client.DOB,
                    Gender = client.Gender,
                    Contact = client.Contact,
                    IsDiabled = client.IsDiabled
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving clients.", ex);
            }
        }

        public async Task<ClientDto?> GetClientByIdAsync(int clientId)
        {
            if (clientId <= 0)
                throw new ArgumentException("Invalid Client ID.");

            try
            {
                var client = await _clientRepo.GetClientByIdAsync(clientId);
                if (client == null) return null;

                return new ClientDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    DOB = client.DOB,
                    Gender = client.Gender,
                    Contact = client.Contact,
                    IsDiabled = client.IsDiabled
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving client with ID {clientId}.", ex);
            }
        }

        public async Task<ClientDto?> GetClientByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.");

            try
            {
                var client = await _clientRepo.GetClientByEmailAsync(email);
                if (client == null) return null;

                return new ClientDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    DOB = client.DOB,
                    Gender = client.Gender,
                    Contact = client.Contact,
                    IsDiabled = client.IsDiabled
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving client with email {email}.", ex);
            }
        }

        public async Task<ClientDto?> UpdateClientAsync(int clientId, UpdateClientDto clientDto)
        {
            if (clientId <= 0)
                throw new ArgumentException("Invalid Client ID.");
            if (clientDto == null)
                throw new ArgumentNullException(nameof(clientDto), "Update data cannot be null.");

            try
            {
                var existingClient = await _clientRepo.GetClientByIdAsync(clientId);
                if (existingClient == null) return null;

                existingClient.Name = clientDto.Name;
                existingClient.DOB = clientDto.DOB;
                existingClient.Gender = clientDto.Gender;
                existingClient.Contact = clientDto.Contact;
                existingClient.IsDiabled = clientDto.IsDiabled;

                var updatedClient = await _clientRepo.UpdateClientAsync(existingClient);
                if (updatedClient == null) return null;

                return new ClientDto
                {
                    Id = updatedClient.Id,
                    Name = updatedClient.Name,
                    Email = updatedClient.Email,
                    DOB = updatedClient.DOB,
                    Gender = updatedClient.Gender,
                    Contact = updatedClient.Contact,
                    IsDiabled = updatedClient.IsDiabled
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating client with ID {clientId}.", ex);
            }
        }

        public async Task<bool> DeleteClientAsync(int clientId)
        {
            if (clientId <= 0)
                throw new ArgumentException("Invalid Client ID.");

            try
            {
                return await _clientRepo.DeleteClientAsync(clientId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting client with ID {clientId}.", ex);
            }
        }
    }
}
