using System.Net;
using DeskBook.API.Controllers;
using DeskBook.AppServices.Contracts.City;
using DeskBook.AppServices.Contracts.Floors;
using DeskBook.AppServices.Contracts.Seat;
using DeskBook.AppServices.DTOs.City;
using DeskBook.AppServices.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.City
{
    public class CityControllerTest
    {
        private readonly Mock<ICityServices> _cityServices;
        private readonly CitiesController _citiesController;
        private readonly Mock<IFloorServices> _floorservice;
        private readonly Mock<ISeatServices> _seatservices;
        private readonly Mock<ILogger<CitiesController>> _logger;

        public CityControllerTest()
        {
            _cityServices = new Mock<ICityServices>();
            _logger = new Mock<ILogger<CitiesController>>();
            _seatservices = new Mock<ISeatServices>();
            _floorservice = new Mock<IFloorServices>();
            _citiesController = new CitiesController(_cityServices.Object, _logger.Object, _floorservice.Object, _seatservices.Object);
        }

        [Fact]
        public async Task GetCityDetail_WithSuccessfulResult_ReturnsOkObjectResult()
        {
            // Arrange
            var city = new List<GetCityResultsDto>
        {
                new GetCityResultsDto { CityId = 1, CityName = "Surat" },
                new GetCityResultsDto { CityId = 2, CityName = "Valsad" }
        };

            var responseDto = new ResponseDto<List<GetCityResultsDto>>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = city
            };

            _cityServices.Setup(s => s.GetCityDetail()).ReturnsAsync(responseDto);

            // Act
            var result = await _citiesController.GetCityDetail();

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var actualResponseDto = Assert.IsType<ResponseDto<List<GetCityResultsDto>>>(okResult.Value);
            Assert.Equal(responseDto.Data, actualResponseDto.Data);
        }

        [Fact]
        public async Task GetCities_ReturnsBadRequestResult()
        {
            //Arrange
            var response = new ResponseDto<List<GetCityResultsDto>>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Error = new List<string>
            {
                    { "An error occurred while Getting Cities." }
            }
            };

            _cityServices.Setup(s => s.GetCityDetail()).ReturnsAsync(response);

            // Act
            var result = await _citiesController.GetCityDetail();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var data = Assert.IsType<ResponseDto<List<GetCityResultsDto>>>(badRequestResult.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
            Assert.Equal("An error occurred while Getting Cities.", data.Error.First());
        }

        [Fact]
        public async Task GetCities_ReturnsInternalServerErrorResult()
        {
            //Arrange
            var response = new ResponseDto<List<GetCityResultsDto>>
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Error = new List<string>
            {
                    { "Internal Server Error." }
            }
            };

            _cityServices.Setup(s => s.GetCityDetail()).ReturnsAsync(response);

            // Act
            var result = await _citiesController.GetCityDetail();

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal(response, internalServerErrorResult.Value);
        }
    }
}