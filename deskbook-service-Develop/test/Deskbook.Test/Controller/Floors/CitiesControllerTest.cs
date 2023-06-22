using System.Net;
using DeskBook.API.Controllers;
using DeskBook.AppServices.Contracts.City;
using DeskBook.AppServices.Contracts.Floors;
using DeskBook.AppServices.Contracts.Seat;
using DeskBook.AppServices.DTOs.Floors;
using DeskBook.AppServices.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.Floors
{
    public class CitiesControllerTest
    {
        private readonly Mock<ICityServices> _cityServices;
        private readonly CitiesController _citiesController;
        private readonly Mock<IFloorServices> _floorservice;
        private readonly Mock<ISeatServices> _seatservices;
        private readonly Mock<ILogger<CitiesController>> _logger;

        public CitiesControllerTest()
        {
            _cityServices = new Mock<ICityServices>();
            _logger = new Mock<ILogger<CitiesController>>();
            _floorservice = new Mock<IFloorServices>();
            _seatservices = new Mock<ISeatServices>();
            _citiesController = new CitiesController(_cityServices.Object, _logger.Object, _floorservice.Object, _seatservices.Object);
        }

        [Fact]
        public async Task To_Check_GetFloors_ReturnsOkResultWithFloorList()
        {
            // Arrange
            int cityId = 1;

            var floors = new List<GetFloorResultDto>
            {
                new GetFloorResultDto { FloorId = 1, FloorName = "First" },
                new GetFloorResultDto { FloorId = 2, FloorName = "Second" }
            };

            var responseDto = new ResponseDto<List<GetFloorResultDto>>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = floors
            };

            _floorservice.Setup(s => s.GetAllFloors(cityId)).ReturnsAsync(responseDto);

            // Act
            var result = await _citiesController.GetAllFloors(cityId);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var actualResponseDto = Assert.IsType<ResponseDto<List<GetFloorResultDto>>>(okResult.Value);
            Assert.Equal(responseDto.Data, actualResponseDto.Data);
        }

        [Fact]
        public async Task To_Check_GetFloors_ReturnsBadRequestResult()
        {
            // Arrange
            int cityId = 1;

            var response = new ResponseDto<List<GetFloorResultDto>>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Error = new List<string>
            {
                    { "An error occurred while Getting the floors." }
            }
            };

            _floorservice.Setup(s => s.GetAllFloors(cityId)).ReturnsAsync(response);

            // Act
            var result = await _citiesController.GetAllFloors(cityId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var data = Assert.IsType<ResponseDto<List<GetFloorResultDto>>>(badRequestResult.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
            Assert.Equal("An error occurred while Getting the floors.", data.Error.First());
        }

        [Fact]
        public async Task To_Check_GetFloors_ReturnsInternalServerErrorResult()
        {
            // Arrange
            int cityId = 1;
            var response = new ResponseDto<List<GetFloorResultDto>>
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Error = new List<string>
            {
                    { "Internal Server Error." }
            }
            };

            _floorservice.Setup(s => s.GetAllFloors(cityId)).ReturnsAsync(response);

            // Act
            var result = await _citiesController.GetAllFloors(cityId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal(response, internalServerErrorResult.Value);
        }
    }
}



