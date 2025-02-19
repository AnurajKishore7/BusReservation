using BusApp.DTOs.BookingManage;
using BusApp.Services.Interfaces.BookingManage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        
        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequestDto bookingRequest)
        {
            if (bookingRequest == null)
                return BadRequest("Invalid booking request.");

            try
            {
                var bookingResponse = await _bookingService.CreateBookingAsync(bookingRequest);
                return CreatedAtAction(nameof(GetBookingById), new { bookingId = bookingResponse.BookingId }, bookingResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        
        [HttpGet("{bookingId}")]
        [Authorize(Roles = "Client, Admin, TransportOperator")]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            if (bookingId <= 0)
                return BadRequest("Invalid booking ID.");

            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(bookingId);
                if (booking == null)
                    return NotFound("Booking not found.");

                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        
        [HttpGet("client/{clientId}")]
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> GetBookingsByClientId(int clientId)
        {
            if (clientId <= 0)
                return BadRequest("Invalid client ID.");

            try
            {
                var bookings = await _bookingService.GetBookingsByClientIdAsync(clientId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        
        [HttpGet("confirmed")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetConfirmedBookings()
        {
            try
            {
                var bookings = await _bookingService.GetConfirmedBookingsAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        
        [HttpGet("confirmed/client/{clientId}")]
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> GetConfirmedBookingsByClientId(int clientId)
        {
            if (clientId <= 0)
                return BadRequest("Invalid client ID.");

            try
            {
                var bookings = await _bookingService.GetConfirmedBookingsByClientIdAsync(clientId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        
        [HttpDelete("{bookingId}")]
        [Authorize(Roles = "Client,TransportOperator,Admin")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            if (bookingId <= 0)
                return BadRequest("Invalid booking ID.");

            try
            {
                var result = await _bookingService.CancelBookingAsync(bookingId);
                if (!result)
                    return NotFound("Booking not found or could not be cancelled.");

                return Ok("Booking cancelled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        
        [HttpPut("{bookingId}/status")]
        [Authorize(Roles = "Admin,Client,TransportOperator")]
        public async Task<IActionResult> UpdateBookingStatus(int bookingId, [FromBody] string status)
        {
            if (bookingId <= 0)
                return BadRequest("Invalid booking ID.");

            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Status is required.");

            try
            {
                var result = await _bookingService.UpdateBookingStatusAsync(bookingId, status);
                if (!result)
                    return NotFound("Booking not found or status update failed.");

                return Ok("Booking status updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
