using BusApp.DTOs.ClientManage;
using BusApp.Services.Interfaces.ClientManage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService  clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                var clients = await _clientService.GetAllClientsAsync();
                if (!clients.Any())
                    return NotFound("No clients found.");

                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving clients: {ex.Message}");
            }
        }

        [HttpGet("{clientId}")]
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> GetClientById(int clientId)
        {
            if (clientId <= 0)
                return BadRequest("Invalid Client ID.");

            try
            {
                var client = await _clientService.GetClientByIdAsync(clientId);
                if (client == null)
                    return NotFound($"Client with ID {clientId} not found.");

                return Ok(client);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving client: {ex.Message}");
            }
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> GetClientByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email cannot be empty.");

            try
            {
                var client = await _clientService.GetClientByEmailAsync(email);
                if (client == null)
                    return NotFound($"Client with email {email} not found.");

                return Ok(client);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving client: {ex.Message}");
            }
        }

        [HttpPut("{clientId}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> UpdateClient(int clientId, [FromBody] UpdateClientDto clientDto)
        {
            if (clientId <= 0)
                return BadRequest("Invalid Client ID.");
            if (clientDto == null)
                return BadRequest("Update data cannot be null.");

            try
            {
                var updatedClient = await _clientService.UpdateClientAsync(clientId, clientDto);
                if (updatedClient == null)
                    return NotFound($"Client with ID {clientId} not found.");

                return Ok(updatedClient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating client: {ex.Message}");
            }
        }

        [HttpDelete("{clientId}")]
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> DeleteClient(int clientId)
        {
            if (clientId <= 0)
                return BadRequest("Invalid Client ID.");

            try
            {
                bool isDeleted = await _clientService.DeleteClientAsync(clientId);
                if (!isDeleted)
                    return NotFound($"Client with ID {clientId} not found or could not be deleted.");

                return Ok($"Client with ID {clientId} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting client: {ex.Message}");
            }
        }
    }
}
