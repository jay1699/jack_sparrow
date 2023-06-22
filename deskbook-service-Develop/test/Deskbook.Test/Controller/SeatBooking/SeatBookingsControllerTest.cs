using System.Net;
using DeskBook.API.Controllers;
using DeskBook.AppServices.Contracts.SeatBooking;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.SeatBooking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.SeatBooking
{
    public class SeatBookingsControllerTest
    {
        private readonly Mock<ISeatBookingServices> _seatbookingServices;
        private readonly UserSeatBookingsController _seatbookingsController;
        private readonly Mock<ILogger<UserSeatBookingsController>> _logger;

        public SeatBookingsControllerTest()
        {
            _seatbookingServices = new Mock<ISeatBookingServices>();
            _logger = new Mock<ILogger<UserSeatBookingsController>>();
            _seatbookingsController = new UserSeatBookingsController(_seatbookingServices.Object, _logger.Object);
        }

        [Fact]
        public async Task To_Check_GetBookingSeats_ReturnsOkResult()
        {
            // Arrange
            int pageNo = 1;
            int pageSize = 10;
            string search = "example";
            string sort = "name";

            var expectedResponse = new ResponseDto<List<SeatBookingResponseDto>>
            {
                Data = new List<SeatBookingResponseDto>
        {
            new SeatBookingResponseDto
            {
                Name = "Jane Smith",
                Email = "jane@example.com",
                RequestDate = "02-01-2023",
                AllottedDate = "03-01-2023",
                FloorNo = 2,
                DeskNo = "456"
            }
        },
                StatusCode = (int)HttpStatusCode.OK
            };

            _seatbookingServices
                .Setup(services => services.GetBookingSeat(pageNo, pageSize, search, sort))
                .ReturnsAsync(expectedResponse);

            // Act
            var actionResult = await _seatbookingsController.GetBookingSeats(pageNo, pageSize, search, sort) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);

            var result = actionResult.Value as ResponseDto<List<SeatBookingResponseDto>>;
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Data.Count, result.Data.Count);
        }

        [Fact]
        public async Task To_Check_GetBookingSeats_ReturnsBadRequestResult()
        {
            // Arrange
            int pageNo = 1;
            int pageSize = 10;
            string search = "example";
            string sort = "name";

            var response = new ResponseDto<List<SeatBookingResponseDto>>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Error = new List<string>
        {
            "An error occurred while Getting the Seat Booking."
        }
            };

            _seatbookingServices
                .Setup(s => s.GetBookingSeat(pageNo, pageSize, search, sort))
                .ReturnsAsync(response);

            // Act
            var actionResult = await _seatbookingsController.GetBookingSeats(pageNo, pageSize, search, sort) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal((int)HttpStatusCode.BadRequest, actionResult.StatusCode);

            var result = actionResult.Value as ResponseDto<List<SeatBookingResponseDto>>;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task To_Check_GetBookingSeats_ReturnInternalServererror()
        {
            // Arrange
            int pageNo = 1;
            int pageSize = 10;
            string search = "example";
            string sort = "name";

            var response = new ResponseDto<List<SeatBookingResponseDto>>
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Error = new List<string>
        {
            "Internal Server Error"
        }
            };

            _seatbookingServices
                .Setup(s => s.GetBookingSeat(pageNo, pageSize, search, sort))
                .ReturnsAsync(response);

            // Act
            var actionResult = await _seatbookingsController.GetBookingSeats(pageNo, pageSize, search, sort) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal((int)HttpStatusCode.InternalServerError, actionResult.StatusCode);
            Assert.NotNull(actionResult.Value);

            var result = actionResult.Value as ResponseDto<List<SeatBookingResponseDto>>;
            Assert.NotNull(result);
            Assert.Equal(response.StatusCode, result.StatusCode);
        }
    }
}
