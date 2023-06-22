using System.Net;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.Seat;
using DeskBook.AppServices.Services.Seat;
using DeskBook.Infrastructure.Contracts.Seat;
using DeskBook.Infrastructure.Model.Seat;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.Seat
{
    public class SeatServicesTest
    {
        private readonly Mock<ISeatRepository> _seatRepository;
        private readonly SeatServices _seatServices;
        private readonly Mock<ILogger<SeatServices>> _logger;

        public SeatServicesTest()
        {
            _logger = new Mock<ILogger<SeatServices>>();
            _seatRepository = new Mock<ISeatRepository>();
            _seatServices = new SeatServices(_seatRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task To_AddSeat_WhenSeatAddedWithinLimits_ReturnsSuccessResponse()
        {
            //Arrange
            string employeeId = "1";
            var seatDto = new List<AddSeatRequestDto>
        {
            new AddSeatRequestDto
            {
                SeatNumber = 5,
                ColumnId = 7
            },
            new AddSeatRequestDto
            {
                SeatNumber = 6,
                ColumnId = 7
            }
        };

            _seatRepository.Setup(x => x.GetAllSeatCount(It.IsAny<byte>())).ReturnsAsync(100);
            _seatRepository.Setup(x => x.GetSeatNumberByColumnId(It.IsAny<byte>())).ReturnsAsync(10);
            _seatRepository.Setup(x => x.GetLastSeat(It.IsAny<byte>())).ReturnsAsync(new SeatModel { SeatNumber = 4, ColumnId = 7 });

            var addedSeats = new List<SeatModel>();
            _seatRepository.Setup(x => x.AddSeat(It.IsAny<List<SeatModel>>()))
            .Callback<List<SeatModel>>(seats => addedSeats.AddRange(seats))
            .Returns(Task.CompletedTask);

            //Act
            var result = await _seatServices.AddSeat(seatDto, employeeId);

            //Assert
            Assert.NotNull(result);
            var responseDto = Assert.IsType<ResponseDto<string>>(result);
            Assert.Equal((int)HttpStatusCode.OK, responseDto.StatusCode);
            Assert.Equal("Seat Added Successfully", responseDto.Data);

            // Verify the added seats
            Assert.Equal(2, addedSeats.Count);
            Assert.Equal(5, addedSeats[0].SeatNumber);
            Assert.Equal(6, addedSeats[1].SeatNumber);
        }

        [Fact]
        public async Task To_AddSeat_WhenExceedingSeatLimit_ReturnsResponseDtoWithBadRequest()
        {
            //Arrange
            string employeeId = "1";
            var seatDto = new List<AddSeatRequestDto>
        {
            new AddSeatRequestDto
            {
                SeatNumber = 16,
                ColumnId = 7
            },
            new AddSeatRequestDto
            {
                SeatNumber = 6,
                ColumnId = 7
            }
        };

            _seatRepository.Setup(x => x.GetAllSeatCount(It.IsAny<byte>())).ReturnsAsync(150);
            _seatRepository.Setup(x => x.GetSeatNumberByColumnId(It.IsAny<byte>())).ReturnsAsync(15);
            _seatRepository.Setup(x => x.GetLastSeat(It.IsAny<byte>())).ReturnsAsync(new SeatModel { SeatNumber = 4, ColumnId = 7 });

            var addedSeats = new List<SeatModel>();
            _seatRepository.Setup(x => x.AddSeat(It.IsAny<List<SeatModel>>()))
            .Callback<List<SeatModel>>(seats => addedSeats.AddRange(seats))
            .Returns(Task.CompletedTask);

            //Act
            var result = await _seatServices.AddSeat(seatDto, employeeId);

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Maximum limit to add seat on one Table is 15 and capacity of one floor is 150 seats.", result.Error.First());
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task To_GetSeats_ReturnsListOfSeats()
        {
            // Arrange
            byte floorId = 1;

            var seatData = new List<SeatResponseModel>
            {
                new SeatResponseModel { ColumnName = "A", SeatNumber = 1, SeatId = 1, ColumnId = 1, SeatStatus = "Green" },
                new SeatResponseModel { ColumnName = "B", SeatNumber = 2, SeatId = 2, ColumnId = 1, SeatStatus = "Red" },
                new SeatResponseModel { ColumnName = "C", SeatNumber = 3, SeatId = 3, ColumnId = 1, SeatStatus = "Blue" },
                new SeatResponseModel { ColumnName = "D", SeatNumber = 4, SeatId = 4, ColumnId = 1, SeatStatus = "Yellow" },
                new SeatResponseModel { ColumnName = "E", SeatNumber = 5, SeatId = 5, ColumnId = 1, SeatStatus = "Grey" }
            };

            _seatRepository.Setup(repo => repo.GetSeat(floorId)).ReturnsAsync(seatData);

            // Act
            var response = await _seatServices.GetSeats(floorId);

            // Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Data);
            Assert.NotNull(response.Data.bookedSeat);
            Assert.NotNull(response.Data.availableforBookingSeat);
            Assert.NotNull(response.Data.reservedSeat);
            Assert.NotNull(response.Data.unavailableSeat);
            Assert.NotNull(response.Data.unallocatedSeat);
            Assert.Equal(1, response.Data.availableforBookingSeat.Count);
            Assert.Equal(1, response.Data.reservedSeat.Count);
            Assert.Equal(1, response.Data.bookedSeat.Count);
            Assert.Equal(1, response.Data.unavailableSeat.Count);
            Assert.Equal(1, response.Data.unallocatedSeat.Count);
        }

        [Fact]
        public async Task To_UpdateSeat_WhenCalledWithValidData_ReturnsSuccessResponse()
        {
            // Arrange
            string employeeId = "1";
            var seats = new List<UpdateSeatRequestDto>
        {
            new UpdateSeatRequestDto
            {
                SeatId = 1,
                IsAvailable = false,
                Unassigned = false
            },
            new UpdateSeatRequestDto
            {
                SeatId = 2,
                Unassigned = true,
                IsAvailable = true
            }
        };

            var seatModel1 = new SeatModel
            {
                SeatId = 1,
                IsAvailable = false
            };
            var seatModel2 = new SeatModel
            {
                SeatId = 2,
                IsAvailable = true
            };
            var seatConfiguration1 = new SeatConfigurationModel
            {
                SeatId = 1
            };
            var seatConfiguration2 = new SeatConfigurationModel
            {
                SeatId = 2
            };

            _seatRepository.Setup(r => r.GetSeatById(1)).ReturnsAsync(seatModel1);
            _seatRepository.Setup(r => r.GetSeatById(2)).ReturnsAsync(seatModel2);

            _seatRepository.Setup(r => r.GetSeatConfigurationById(1)).ReturnsAsync(seatConfiguration1);
            _seatRepository.Setup(r => r.GetSeatConfigurationById(2)).ReturnsAsync(seatConfiguration2);

            _seatRepository.Setup(r => r.UpdateSeat(It.IsAny<List<SeatModel>>()));
            _seatRepository.Setup(r => r.UpdateSeatConfiguration(It.IsAny<List<SeatConfigurationModel>>()));

            // Act
            var result = await _seatServices.UpdateSeat(seats, employeeId);

            // Assert
            _seatRepository.Verify(r => r.UpdateSeat(It.IsAny<List<SeatModel>>()), Times.Once);
            _seatRepository.Verify(r => r.UpdateSeatConfiguration(It.IsAny<List<SeatConfigurationModel>>()), Times.Never);

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task To_UpdateSeat_WhenCalledWithInValidData_ReturnsResponseDtoWithBadRequest()
        {
            // Arrange
            string employeeId = "1";
            var seats = new List<UpdateSeatRequestDto>
            {
                new UpdateSeatRequestDto
                {
                    SeatId = 1,
                    IsAvailable = true
                },
                new UpdateSeatRequestDto
                {
                    SeatId = 2,
                    Unassigned = true
                }
            };

            _seatRepository.Setup(r => r.GetSeatById(It.IsAny<int>())).ReturnsAsync((SeatModel)null);
            _seatRepository.Setup(r => r.GetSeatConfigurationById(It.IsAny<int>())).ReturnsAsync((SeatConfigurationModel)null);

            // Act
            var result = await _seatServices.UpdateSeat(seats, employeeId);

            // Assert
            _seatRepository.Verify(r => r.UpdateSeat(It.IsAny<List<SeatModel>>()), Times.Never);
            _seatRepository.Verify(r => r.UpdateSeatConfiguration(It.IsAny<List<SeatConfigurationModel>>()), Times.Never);

            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Seat status can not be changed", result.Error[0]);
            Assert.Null(result.Data);
        }
    }
}