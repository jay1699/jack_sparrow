using System.Net;
using DeskBook.AppServices.Contracts.SeatBooking;
using Microsoft.AspNetCore.Mvc;

namespace DeskBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSeatBookingsController : ControllerBase
    {
        private readonly ISeatBookingServices _seatBookingServices;

        private readonly ILogger<UserSeatBookingsController> _logger;

        public UserSeatBookingsController(ISeatBookingServices seatBookingServices, ILogger<UserSeatBookingsController> logger)
        {
            _seatBookingServices = seatBookingServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookingSeats([FromQuery] int pageNo, [FromQuery] int pageSize, [FromQuery] string? search, [FromQuery] string? sort)
        {
            var result = await _seatBookingServices.GetBookingSeat(pageNo, pageSize, search, sort);

            if (result != null && result.StatusCode == (int)HttpStatusCode.OK)
            {
                _logger.LogError("Getting BookingHistory Successfully");
                return StatusCode(200, result);
            }
            else if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("Error while Getting BookingHistory");
                return BadRequest(result);
            }
            else
            {
                _logger.LogError("Internal Server Error");
                return StatusCode(500, result);
            }
        }
    }
}