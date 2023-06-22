using System.Net;
using DeskBook.AppServices.Contracts.Seat;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.Seat;
using DeskBook.AppServices.Extension;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DeskBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(CustomValidationExceptionFilter))]
    public class SeatsController : BaseController
    {
        private readonly ISeatServices _seatServices;

        private readonly ILogger<SeatsController> _logger;

        public SeatsController(ISeatServices seatServices, ILogger<SeatsController> logger)
        {
            _seatServices = seatServices;
            _logger = logger;
        }

        [SwaggerResponse(200, Type = typeof(ResponseDto<object>))]
        [SwaggerResponse(500, Type = typeof(ResponseDto<object>))]
        [SwaggerResponse(400, Type = typeof(ResponseDto<object>))]
        [HttpPost]
        public async Task<IActionResult> AddSeat([FromBody] List<AddSeatRequestDto> seatDto)
        {
            var result = await _seatServices.AddSeat(seatDto, UserRegisteredId);
            if (result != null && result.StatusCode == (int)HttpStatusCode.OK)
            {
                _logger.LogError("Seat Added Successfully");
                return StatusCode(200, result);
            }
            else if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("Maximum limit to add seat on one Table is 15 and capacity of one floor is 150 seats.");
                return BadRequest(result);
            }
            else
            {
                _logger.LogError("Internal Server Error");
                return StatusCode(500, result);
            }
        }

        [SwaggerResponse(200, Type = typeof(ResponseDto<object>))]
        [SwaggerResponse(500, Type = typeof(ResponseDto<object>))]
        [SwaggerResponse(400, Type = typeof(ResponseDto<object>))]
        [HttpPut]
        public async Task<IActionResult> UpdateSeat([FromBody] List<UpdateSeatRequestDto> seats)
        {
            var result = await _seatServices.UpdateSeat(seats, UserRegisteredId);

            if (result != null && result.StatusCode == (int)HttpStatusCode.OK)
            {
                _logger.LogInformation("Seat status has been changed to Unavailable");
                _logger.LogInformation("Seat status has been changed to Unassigned");
                return StatusCode(200, result);
            }
            else if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("Seat status can not be changed");
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