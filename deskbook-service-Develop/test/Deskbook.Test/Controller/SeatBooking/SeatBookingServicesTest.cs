using System.Net;
using DeskBook.AppServices.Services.SeatBooking;
using DeskBook.Infrastructure.Contracts.SeatBooking;
using DeskBook.Infrastructure.Model.SeatRequest;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.SeatBooking
{
    public class SeatBookingServicesTest
    {
        private readonly SeatBookingServices _seatbookingServices;
        private readonly Mock<ISeatBookingRepository> _seatbookingRepository;
        private readonly Mock<ILogger<SeatBookingServices>> _logger;

        public SeatBookingServicesTest()
        {
            _logger = new Mock<ILogger<SeatBookingServices>>();
            _seatbookingRepository = new Mock<ISeatBookingRepository>();
            _seatbookingServices = new SeatBookingServices(_seatbookingRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task To_Check_GetBookingSeat_ReturnsResponseWithOkStatusCode()
        {
            // Arrange
            int pageNo = 1;
            int pageSize = 10;
            string search = "example";
            string sort = "name";

            var SeatBookingRepository = new Mock<ISeatBookingRepository>();
            var expectedSeats = new List<SeatBookingResponseModel>
    {
        new SeatBookingResponseModel
        {
            Name = "John Doe",
            Email = "john@example.com",
            RequestDate = DateTime.Now,
            AllottedDate = DateTime.Now.AddDays(1),
            FloorNo = 1,
            DeskNo = "123"
        },
        new SeatBookingResponseModel
        {
            Name = "Jane Smith",
            Email = "jane@example.com",
            RequestDate = DateTime.Now.AddDays(-1),
            AllottedDate = DateTime.Now,
            FloorNo = 2,
            DeskNo = "456"
        }
    };
            _seatbookingRepository
                .Setup(repo => repo.GetBookingSeat(pageNo, pageSize, search, sort))
                .ReturnsAsync(expectedSeats);

            // Act
            var result = await _seatbookingServices.GetBookingSeat(pageNo, pageSize, search, sort);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedSeats.Count, result.Data.Count);
            Assert.Equal(expectedSeats[0].Name, result.Data[0].Name);
            Assert.Equal(expectedSeats[1].Email, result.Data[1].Email);
            // Assert other properties...

            _seatbookingRepository.Verify(repo =>
                repo.GetBookingSeat(pageNo, pageSize, search, sort), Times.Once);
        }

        [Fact]
        public async Task To_Check_GetBookingSeat_ReturnResponseWithBadRequestStatusCode()
        {
            // Arrange
            int pageNo = 1;
            int pageSize = 10;
            string search = "example";
            string sort = "name";

            // Mock the seat booking repository to return null or empty list, simulating a bad request scenario
            _seatbookingRepository.Setup(repo => repo.GetBookingSeat(pageNo, pageSize, search, sort))
                .ReturnsAsync((List<SeatBookingResponseModel>)null);

            // Act
            var result = await _seatbookingServices.GetBookingSeat(pageNo, pageSize, search, sort);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Null(result.Data);
        }
    }
}
