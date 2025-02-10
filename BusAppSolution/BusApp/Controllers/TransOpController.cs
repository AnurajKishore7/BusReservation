using BusApp.DTOs.TransOP;
using BusApp.Services.Interfaces.TransOp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransOpController : ControllerBase
    {
        private readonly ITransOpService _transOpService;

        public TransOpController(ITransOpService transOpService)
        {
            _transOpService = transOpService;
        }

        [HttpGet("{operatorId}")]
        [Authorize(Roles = "Admin,TransportOperator")]
        public async Task<IActionResult> GetTransportOperatorById(int operatorId)
        {
            try
            {
                var result = await _transOpService.GetOperatorByIdAsync(operatorId);
                if (result == null)
                    return NotFound("Transport Operator not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("{operatorId}")]
        [Authorize(Roles = "TransportOperator")]
        public async Task<IActionResult> UpdateTransportOperator(int operatorId, [FromBody] TransOpUpdateDto updateDto)
        {
            try
            {
                var isUpdated = await _transOpService.UpdateOperatorAsync(operatorId, updateDto);
                if (!isUpdated)
                    return NotFound("Transport Operator not found or update failed.");

                return Ok("Transport Operator details updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{operatorId}")]
        [Authorize(Roles = "Admin,TransportOperator")]
        public async Task<IActionResult> DeleteTransportOperator(int operatorId)
        {
            try
            {
                var isDeleted = await _transOpService.DeleteOperatorAsync(operatorId);
                if (!isDeleted)
                    return NotFound("Transport Operator not found or deletion failed.");

                return Ok("Transport Operator deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
