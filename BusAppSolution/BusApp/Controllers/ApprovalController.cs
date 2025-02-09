using BusApp.DTOs;
using BusApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ApprovalController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public ApprovalController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPut("approve-transport-operator")]
        public async Task<IActionResult> ApproveTransportOperator([FromBody] ApproveRoleDto approvalDto)
        {
            try
            {
                var result = await _roleService.ApproveTransportOperator(approvalDto);

                if (!result)
                    return NotFound(new { message = "User not found" });

                return Ok(new { message = "User role updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while processing your request." });
            }
        }
    }
}
