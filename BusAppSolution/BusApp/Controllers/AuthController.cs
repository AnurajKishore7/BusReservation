using BusApp.DTOs.Auth;
using BusApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register-client")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterClient([FromBody] ClientRegisterDto clientDto)
        {
            try
            {
                var result = await _userService.RegisterClient(clientDto);
                if (result == null)
                    return BadRequest(new { message = "Email already exists" });

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPost("register-transport-operator")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterTransportOperator([FromBody] TransportOperatorRegisterDto operatorDto)
        {
            try
            {
                var result = await _userService.RegisterTransportOperator(operatorDto);
                if (result == null)
                    return BadRequest(new { message = "Email already exists" });

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _userService.Login(loginDto);
                if (result == null)
                    return Unauthorized(new { message = "Invalid credentials" });

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while processing your request." });
            }
        }
    }
}
