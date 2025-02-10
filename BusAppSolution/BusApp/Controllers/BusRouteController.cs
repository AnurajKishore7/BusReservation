using BusApp.DTOs.BusRouteManage;
using BusApp.Services.Interfaces.BusRouteManage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusRouteController : ControllerBase
    {
        private readonly IBusRouteService _busRouteService;

        public BusRouteController(IBusRouteService busRouteService)
        {
            _busRouteService = busRouteService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllRoutes()
        {
            try
            {
                var routes = await _busRouteService.GetAllRoutesAsync();
                return Ok(routes);
            }
            catch
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving bus routes." });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRouteById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { Message = "Invalid route ID." });

                var route = await _busRouteService.GetRouteByIdAsync(id);
                if (route == null)
                    return NotFound(new { Message = $"Route with ID {id} not found." });

                return Ok(route);
            }
            catch
            {
                return StatusCode(500, new { Message = $"An error occurred while fetching route with ID {id}." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoute([FromBody] BusRouteAddDto routeDto)
        {
            try
            {
                if (routeDto == null)
                    return BadRequest(new { Message = "Route data is required." });

                var createdRoute = await _busRouteService.AddRouteAsync(routeDto);
                return CreatedAtAction(nameof(GetRouteById), new { id = createdRoute.Id }, createdRoute);
            }
            catch
            {
                return StatusCode(500, new { Message = "An error occurred while adding the bus route." });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoute([FromBody] BusRouteUpdateDto routeDto)
        {
            try
            {
                if (routeDto == null)
                    return BadRequest(new { Message = "Route data is required." });

                var updatedRoute = await _busRouteService.UpdateRouteAsync(routeDto);
                if (updatedRoute == null)
                    return NotFound(new { Message = $"Route with ID {routeDto.Id} not found." });

                return Ok(updatedRoute);
            }
            catch
            {
                return StatusCode(500, new { Message = "An error occurred while updating the bus route." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { Message = "Invalid route ID." });

                bool isDeleted = await _busRouteService.DeleteRouteAsync(id);
                if (!isDeleted)
                    return NotFound(new { Message = $"Route with ID {id} not found." });

                return Ok(new { Message = "Route deleted successfully." });
            }
            catch
            {
                return StatusCode(500, new { Message = $"An error occurred while deleting route with ID {id}." });
            }
        }
    }
}
