using System.Net;
using DeskBook.API.Controllers;
using DeskBook.AppServices.Contracts.SeatBooking;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.SeatBooking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.UserSeatBooking
{
    public class UserSeatBookingControllerTest
    {
        private readonly Mock<ISeatBookingServices> _seatBookingServices;
        private readonly Mock<ILogger<UserSeatBookingsController>> _logger;
        private readonly UserSeatBookingsController _controller;

        public UserSeatBookingControllerTest()
        {
            _seatBookingServices = new Mock<ISeatBookingServices>();
            _logger = new Mock<ILogger<UserSeatBookingsController>>();
            _controller = new UserSeatBookingsController(_seatBookingServices.Object, _logger.Object);
        }

        [Fact]
        public async Task GetBookingSeats_WithValidData_ReturnsOkResult()
        {
            // Arrange
            int pageNo = 1;
            int pageSize = 10;
            string search = "example";
            string sort = "asc";
            var seats = new List<SeatBookingResponseDto>
    {
    new SeatBookingResponseDto
    {
        Name = "Jay Patel",
        Email = "jay.patel@example.com",
        RequestDate = DateTime.Now.ToString("dd-MM-yyyy"),
        AllottedDate = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy"),
        FloorNo = 1,
        DeskNo = "A1"
    },
    new SeatBookingResponseDto
    {
        Name = "Harsh Raghav",
        Email = "Harsh.Raghav@example.com",
        RequestDate = DateTime.Now.ToString("dd-MM-yyyy"),
        AllottedDate = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy"),
        FloorNo = 2,
        DeskNo = "B2"
    }
    };

            var response = new ResponseDto<List<SeatBookingResponseDto>>()
            {
                Data = seats,
                StatusCode = (int)HttpStatusCode.OK
            };
            _seatBookingServices.Setup(s => s.GetBookingSeat(pageNo, pageSize, search, sort)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetBookingSeats(pageNo, pageSize, search, sort) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task To_GetBookingSeats_WithValidData_ReturnsBadResult()
        {
            // Arrange
            int pageNo = 1;
            int pageSize = 10;
            string search = "ex";
            string sort = "Past Booking";

            var badResponse = new ResponseDto<List<SeatBookingResponseDto>>()
            {
                Data = null,
                Error = new List<string> { "Minimum three character required" },
                StatusCode = (int)HttpStatusCode.BadRequest
            };
            _seatBookingServices.Setup(s => s.GetBookingSeat(pageNo, pageSize, search, sort)).ReturnsAsync(badResponse);

            // Act
            var result = await _controller.GetBookingSeats(pageNo, pageSize, search, sort) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(badResponse, result.Value);
        }
    }
}
