using System.Net;
using DeskBook.AppServices.Contracts.City;
using DeskBook.AppServices.Contracts.Floors;
using DeskBook.AppServices.Contracts.Seat;
using Microsoft.AspNetCore.Mvc;

namespace DeskBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityServices _cityServices;
        private readonly ILogger<CitiesController> _logger;
        private readonly IFloorServices _floorservice;
        private readonly ISeatServices _seatservices;

        public CitiesController(ICityServices cityServices, ILogger<CitiesController> logger, IFloorServices floorservice, ISeatServices seatservices)

        {
            _cityServices = cityServices;
            _logger = logger;
            _floorservice = floorservice;
            _seatservices = seatservices;
        }

        [HttpGet]
        public async Task<IActionResult> GetCityDetail()
        {
            var result = await _cityServices.GetCityDetail();

            if (result != null && result.StatusCode == (int)HttpStatusCode.OK)
            {
                _logger.LogError("Getting Cities Successfully");
                return StatusCode(200, result);
            }
            else if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("Error while Getting Cities");
                return BadRequest(result);
            }
            else
            {
                _logger.LogError("Internal Server Error");
                return StatusCode(500, result);
            }
        }

        [HttpGet("{cityId}/floors")]
        public async Task<IActionResult> GetAllFloors(int cityId)
        {
            var result = await _floorservice.GetAllFloors(cityId);

            if (result != null && result.StatusCode == (int)HttpStatusCode.OK)
            {
                _logger.LogError("Getting City wise Floor Successfully");
                return StatusCode(200, result);
            }
            else if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("Error while Getting City Wise Floor");
                return BadRequest(result);
            }
            else
            {
                _logger.LogError("Internal Server Error");
                return StatusCode(500, result);
            }
        }

        [HttpGet("{cityId}/floors/{floorId}/seats")]
        public async Task<IActionResult> GetBookedSeat(byte floorId)
        {
            var result = await _seatservices.GetSeats(floorId);
            if (result != null && result.StatusCode == (int)HttpStatusCode.OK)
            {
                _logger.LogError("Getting Seats Successfully.");
                return StatusCode(200, result);
            }
            else if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("Error while Getting Seats");
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