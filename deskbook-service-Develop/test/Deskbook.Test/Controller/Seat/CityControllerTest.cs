using System.Net;
using DeskBook.API.Controllers;
using DeskBook.AppServices.Contracts.City;
using DeskBook.AppServices.Contracts.Floors;
using DeskBook.AppServices.Contracts.Seat;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.Seat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.Seat
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityControllerTest : ControllerBase
    {
        private readonly Mock<ISeatServices> _seatServices;
        private readonly Mock<IFloorServices> _floorServices;
        private readonly Mock<ICityServices> _cityServices;
        private readonly CitiesController _citiesController;
        private readonly Mock<ILogger<CitiesController>> _logger;

        public CityControllerTest()
        {
            _seatServices = new Mock<ISeatServices>();
            _floorServices = new Mock<IFloorServices>();
            _cityServices = new Mock<ICityServices>();
            _logger = new Mock<ILogger<CitiesController>>();
            _citiesController = new CitiesController(_cityServices.Object, _logger.Object, _floorServices.Object, _seatServices.Object);
        }

        [Fact]
        public async Task To_Check_GetSeats_ReturnOkResult()
        {
            // Arrange
            byte floorId = 1;

            var responseDto = new ResponseDto<GetSeatResponseDto>
            {
                Data = new GetSeatResponseDto(),
                StatusCode = (int)HttpStatusCode.OK
            };
            _seatServices.Setup(s => s.GetSeats(floorId)).ReturnsAsync(responseDto);

            // Act
            var result = await _citiesController.GetBookedSeat(floorId);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var response = Assert.IsType<ResponseDto<GetSeatResponseDto>>(okResult.Value);
            Assert.Equal(responseDto.Data, response.Data);
        }

        [Fact]
        public async Task To_Check_GetSeats_ReturnBadRequestResult()
        {
            // Arrange
            byte floorId = 1;

            var responseDto = new ResponseDto<GetSeatResponseDto>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Error = new List<string>
                {
                    {"Error while Getting Seats" }
                }
            };

            _seatServices.Setup(s => s.GetSeats(floorId)).ReturnsAsync(responseDto);

            // Act
            var result = await _citiesController.GetBookedSeat(floorId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
            var response = Assert.IsType<ResponseDto<GetSeatResponseDto>>(badRequestResult.Value);
            Assert.Equal("Error while Getting Seats", response.Error.First());
        }

        [Fact]
        public async Task To_Check_GetSeats_ReturnInternalServerError()
        {
            // Arrange
            byte floorId = 1;

            var responseDto = new ResponseDto<GetSeatResponseDto>
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Error = new List<string>
                {
                    {"Internal Server Error" }
                }
            };

            _seatServices.Setup(s => s.GetSeats(floorId)).ReturnsAsync(responseDto);

            // Act
            var result = await _citiesController.GetBookedSeat(floorId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal(responseDto, internalServerErrorResult.Value);
        }
    }
}
