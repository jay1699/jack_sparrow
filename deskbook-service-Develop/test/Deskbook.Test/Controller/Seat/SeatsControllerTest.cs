using System.Net;
using DeskBook.API.Controllers;
using DeskBook.AppServices.Contracts.Seat;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.Seat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.Seat
{
    public class SeatsControllerTest
    {
        private readonly Mock<ISeatServices> _seatServices;
        private readonly SeatsController _seatsController;
        private readonly Mock<ILogger<SeatsController>> _logger;

        public SeatsControllerTest()
        {
            _seatServices = new Mock<ISeatServices>();
            _logger = new Mock<ILogger<SeatsController>>();
            _seatsController = new SeatsController(_seatServices.Object, _logger.Object);
        }

        [Fact]
        public async Task To_Check_AddSeat_WhenCalledWithValidData_ShouldReturnOkResult()
        {
            // Arrange
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
            },
        };

            _seatServices.Setup(s => s.AddSeat(It.IsAny<List<AddSeatRequestDto>>(), It.IsAny<string>()))
                 .ReturnsAsync(new ResponseDto<string>
                 {
                     StatusCode = (int)HttpStatusCode.OK,
                     Data = "Seat Added Successfully"
                 });

            // Act
            var result = await _seatsController.AddSeat(seatDto);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(typeof(ResponseDto<string>), okResult.Value.GetType());
            var responseDto = (ResponseDto<string>)okResult.Value;
            Assert.Equal("Seat Added Successfully", responseDto.Data);
        }

        [Fact]
        public async Task To_Check_AddSeat_WhenCalledWithExceedingSeatLimit_ShouldReturnBadRequestResult()
        {
            //Arrange
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
            },
        };

            var expectedErrorMessage = "Maximum limit to add seat on one Table is 15 and capacity of one floor is 150 seats.";

            _seatServices.Setup(s => s.AddSeat(It.IsAny<List<AddSeatRequestDto>>(), It.IsAny<string>())).ReturnsAsync(new ResponseDto<string>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Error = new List<string> { expectedErrorMessage }
            });

            //Act
            var result = await _seatsController.AddSeat(seatDto);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestObjectResult.StatusCode);
            var responseDto = Assert.IsType<ResponseDto<string>>(badRequestObjectResult.Value);
            Assert.Equal(expectedErrorMessage, responseDto.Error.First());
        }

        [Fact]
        public async Task To_Check_AddSeat_WhenAddSeatServiceReturnsNull_ShouldReturnInternalServerError()
        {
            //Arrange
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
            },
        };

            _seatServices.Setup(s => s.AddSeat(It.IsAny<List<AddSeatRequestDto>>(), It.IsAny<string>())).ReturnsAsync((ResponseDto<string>)null);

            //Act
            var result = await _seatsController.AddSeat(seatDto);

            //Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task To_Check_UpdateSeat_WhenCalledWithValidData_ShouldReturnOkResult()
        {
            // Arrange
            var seats = new List<UpdateSeatRequestDto>
        {
            new UpdateSeatRequestDto
            {
                SeatId = 1,
                IsAvailable = false
            },
            new UpdateSeatRequestDto
            {
                SeatId = 2,
                Unassigned = true
            }
        };

            _seatServices.Setup(s => s.UpdateSeat(It.IsAny<List<UpdateSeatRequestDto>>(), It.IsAny<string>()))

                            .ReturnsAsync(new ResponseDto<string>
                            {
                                StatusCode = (int)HttpStatusCode.OK,
                                Data = "Seat status has been changed to Unavailable"
                            });

            // Act
            var result = await _seatsController.UpdateSeat(seats);

            // Assert
            var okResult = result as ObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(typeof(ResponseDto<string>), okResult.Value.GetType());
            var responseDto = (ResponseDto<string>)okResult.Value;
            Assert.Equal("Seat status has been changed to Unavailable", responseDto.Data);
        }

        [Fact]
        public async Task To_Check_UpdateSeat_WhenCalledWithInValidData_ShouldReturnBadRequestResult()
        {
            // Arrange
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

            var expectedErrorMessage = "Seat status can not be changed";
            _seatServices.Setup(s => s.UpdateSeat(It.IsAny<List<UpdateSeatRequestDto>>(), It.IsAny<string>()))
                            .ReturnsAsync(new ResponseDto<string>
                            {
                                StatusCode = (int)HttpStatusCode.BadRequest,
                                Error = new List<string> { expectedErrorMessage }
                            });

            // Act
            var result = await _seatsController.UpdateSeat(seats);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestObjectResult.StatusCode);
            var responseDto = Assert.IsType<ResponseDto<string>>(badRequestObjectResult.Value);
            Assert.Equal(expectedErrorMessage, responseDto.Error.First());
        }

        [Fact]
        public async Task To_Check_UpdateSeat_WhenUpdateSeatServiceReturnsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            var seats = new List<UpdateSeatRequestDto>
        {
            new UpdateSeatRequestDto
            {
                SeatId = 1,
                IsAvailable = false
            },
            new UpdateSeatRequestDto
            {
                SeatId = 2,
                Unassigned = true
            }
        };

            _seatServices.Setup(s => s.UpdateSeat(It.IsAny<List<UpdateSeatRequestDto>>(), It.IsAny<string>()))
                            .ReturnsAsync((ResponseDto<string>)null);

            // Act
            var result = await _seatsController.UpdateSeat(seats);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }
    }
}