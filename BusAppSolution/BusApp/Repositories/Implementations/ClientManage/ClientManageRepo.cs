using BusApp.Models;
using BusApp.Repositories.Interfaces.ClientManage;
using BusReservationApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations.ClientManage
{
    public class ClientManageRepo : IClientManageRepo
    {
        private readonly AppDbContext _context;

        public ClientManageRepo(AppDbContext context)
        {
            _context = context;  
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            try
            {
                var clients = await _context.Clients.ToListAsync();
                return clients ?? new List<Client>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all clients.", ex);
            }
        }

        public async Task<Client?> GetClientByIdAsync(int clientId)
        {
            try
            {
                if (clientId <= 0)
                    throw new ArgumentException("Invalid Client ID.");

                var client = await _context.Clients.FindAsync(clientId);
                return client ?? null;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching client with ID {clientId}.", ex);
            }
        }

        public async Task<Client?> GetClientByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Email cannot be null or empty.");

                var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == email);
                return client ?? null;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching client with email {email}.", ex);
            }
        }

        public async Task<Client?> UpdateClientAsync(Client client)
        {
            try
            {
                if (client == null)
                    throw new ArgumentNullException(nameof(client), "Client data cannot be null.");

                var existingClient = await _context.Clients.FindAsync(client.Id);
                if (existingClient == null)
                    return null;

                // Update fields
                existingClient.Name = client.Name;
                existingClient.DOB = client.DOB;
                existingClient.Gender = client.Gender;
                existingClient.Contact = client.Contact;
                existingClient.IsDiabled = client.IsDiabled;

                _context.Clients.Update(existingClient);
                await _context.SaveChangesAsync();

                return existingClient;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating client with ID {client.Id}.", ex);
            }
        }

        public async Task<bool> DeleteClientAsync(int clientId)
        {
            try
            {
                if (clientId <= 0)
                    throw new ArgumentException("Invalid Client ID.");

                var client = await _context.Clients.FindAsync(clientId);
                if (client == null)
                    return false; 

                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();

                return true; 
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting client with ID {clientId}.", ex);
            }
        }


    }
}
