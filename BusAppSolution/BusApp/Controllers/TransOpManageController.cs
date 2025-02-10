using System.Drawing.Printing;
using BusApp.DTOs.BusManagement;
using System.Net;
using BusApp.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransOpManageController : ControllerBase
    {
        private readonly ITransOpManageService _transOpManageService;
        public TransOpManageController(ITransOpManageService transOpManageService)
        {
            _transOpManageService = transOpManageService; 
        }

        
        [HttpPost("add-bus")]
        [Authorize(Roles = "TransportOperator")]  // Only transport operators can add buses
        public async Task<IActionResult> AddBus([FromBody] BusDto busDto)
        {
            try
            {
                if (busDto == null)
                    return BadRequest("Bus data cannot be null.");

                bool result = await _transOpManageService.AddBusAsync(busDto);
                if (!result)
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to add bus.");

                return Ok(new { Message = "Bus added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("operator/{operatorId}/buses")]
        [Authorize(Roles = "TransportOperator,Admin")]  // Admins can also view all operator buses
        public async Task<IActionResult> GetBusesByOperatorId(int operatorId)
        {
            try
            {
                var buses = await _transOpManageService.GetBusesByOperatorIdAsync(operatorId);
                if (!buses.Any())
                    return NotFound($"No buses found for operator ID {operatorId}.");

                return Ok(buses);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        
        [HttpGet("bus/{busId}")]
        public async Task<IActionResult> GetBusById(int busId)
        {
            try
            {
                var bus = await _transOpManageService.GetBusByIdAsync(busId);
                if (bus == null)
                    return NotFound($"Bus with ID {busId} not found.");

                return Ok(bus);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        
        [HttpPut("update-bus/{busId}")]
        [Authorize(Roles = "TransportOperator")]
        public async Task<IActionResult> UpdateBus(int busId, [FromBody] BusDto busDto)
        {
            try
            {
                bool updated = await _transOpManageService.UpdateBusAsync(busId, busDto);
                if (!updated)
                    return NotFound($"Bus with ID {busId} not found or update failed.");

                return Ok(new { Message = "Bus updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        
        [HttpDelete("delete-bus/{busId}")]
        [Authorize(Roles = "TransportOperator,Admin")]  // Admins can also delete buses if needed
        public async Task<IActionResult> DeleteBus(int busId)
        {
            try
            {
                bool deleted = await _transOpManageService.DeleteBusAsync(busId);
                if (!deleted)
                    return NotFound($"Bus with ID {busId} not found or could not be deleted.");

                return Ok(new { Message = "Bus deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
