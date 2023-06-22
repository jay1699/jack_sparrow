using System.Net;
using DeskBook.AppServices.Contracts.SeatRequest;
using DeskBook.AppServices.DTOs.SeatRequest;
using Microsoft.AspNetCore.Mvc;

namespace DeskBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSeatRequestsController : BaseController
    {
        private readonly IUserSeatRequestServices _userSeatRequestServices;
        private readonly ILogger<UserSeatRequestsController> _logger;

        public UserSeatRequestsController(IUserSeatRequestServices userSeatRequestServices, ILogger<UserSeatRequestsController> logger)
        {
            _userSeatRequestServices = userSeatRequestServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSeatRequest([FromQuery] int pageNo, [FromQuery] int pageSize, [FromQuery] string search, [FromQuery] string sort)
        {
            var result = await _userSeatRequestServices.GetAllSeatRequest(pageNo, pageSize, search, sort);
            if (result != null && result.StatusCode == (int)HttpStatusCode.OK)
            {
                _logger.LogError("All Seat Rquests are retrieved successfully.");
                return StatusCode(200, result);
            }
            else if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("Error while Getting Seat Request");
                return BadRequest(result);
            }
            else
            {
                _logger.LogError("Internal Server Error");
                return StatusCode(500, result);
            }
        }

        [HttpGet("{seatRequestId}/employee")]
        public async Task<IActionResult> GetSeatRequestById(int seatRequestId)
        {
            var result = await _userSeatRequestServices.GetSeatRequestById(seatRequestId);
            if (result != null && result.StatusCode == (int)HttpStatusCode.OK)
            {
                _logger.LogError("Employee Deatils Retrieved Sucessfully");
                return StatusCode(200, result);
            }
            else if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("Error while Getting Employee Deatils");
                return BadRequest(result);
            }
            else
            {
                _logger.LogError("Internal Server Error");
                return StatusCode(500, result);
            }
        }

        [HttpPut("{seatRequestId}/employee")]
        public async Task<IActionResult> UpdateSeatRequest(int seatRequestId, UpdateUserSeatRequestDto seatRequestdto)
        {
            var result = await _userSeatRequestServices.UpdateSeatRequest(seatRequestId, seatRequestdto, UserRegisteredId);
            if (result != null && result.StatusCode == (int)HttpStatusCode.OK)
            {
                _logger.LogError("Successfull");
                return StatusCode(200, result);
            }
            else if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("Bad Request");
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
