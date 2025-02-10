using BusApp.DTOs.TripManage;
using BusApp.Services.Interfaces.TripManage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _service;

        public TripController(ITripService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTrips()
        {
            try
            {
                var trips = await _service.GetAllTripsAsync();
                return Ok(trips);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching trips.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTripById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid trip ID." });

                var trip = await _service.GetTripByIdAsync(id);
                if (trip == null)
                    return NotFound(new { message = "Trip not found." });

                return Ok(trip);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the trip.", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,TransportOperator")]
        public async Task<IActionResult> AddTrip([FromBody] TripAddDto tripDto)
        {
            try
            {
                if (tripDto == null)
                    return BadRequest(new { message = "Trip data cannot be null." });

                var trip = await _service.AddTripAsync(tripDto);
                return CreatedAtAction(nameof(GetTripById), new { id = trip.Id }, trip);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the trip.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,TransportOperator")]
        public async Task<IActionResult> UpdateTrip(int id, [FromBody] TripUpdateDto tripDto)
        {
            try
            {
                if (id != tripDto.Id)
                    return BadRequest(new { message = "Trip ID in URL does not match with body." });

                var updatedTrip = await _service.UpdateTripAsync(tripDto);
                if (updatedTrip == null)
                    return NotFound(new { message = "Trip not found or could not be updated." });

                return Ok(updatedTrip);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the trip.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,TransportOperator")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid trip ID." });

                var isDeleted = await _service.DeleteTripAsync(id);
                if (!isDeleted)
                    return NotFound(new { message = "Trip not found or could not be deleted." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the trip.", error = ex.Message });
            }
        }
    }
}
