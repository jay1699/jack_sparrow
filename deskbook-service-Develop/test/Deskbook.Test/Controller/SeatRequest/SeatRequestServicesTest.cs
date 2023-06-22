using System.Globalization;
using System.Net;
using DeskBook.AppServices.DTOs.SeatRequest;
using DeskBook.AppServices.Services.SeatRequest;
using DeskBook.Infrastructure.Contracts.SeatRequest;
using DeskBook.Infrastructure.Model.EmailModel;
using DeskBook.Infrastructure.Model.SeatRequest;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.SeatRequest
{
    public class SeatRequestServicesTest
    {
        private readonly Mock<IUserSeatRequestRepository> _seatRequestRepository;

        private readonly UserSeatRequestServices _seatRequestServices;

        private readonly Mock<ILogger<UserSeatRequestServices>> _logger;

        private readonly Mock<EmailModel> _emailModel;

        public SeatRequestServicesTest()
        {
            _logger = new Mock<ILogger<UserSeatRequestServices>>();
            _seatRequestRepository = new Mock<IUserSeatRequestRepository>();
            _seatRequestServices = new UserSeatRequestServices(_seatRequestRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task To_GetAllSeatRequest_WithSearchAndSort_ReturnsFilteredAndSortedResults()
        {
            // Arrange
            var search = "John";
            var sort = "Pending";
            var pageNo = 1;
            var pageSize = 10;

            var requestResults = new List<GetSeatRequestResponseModel>
        {
            new GetSeatRequestResponseModel
            {
                Name = "John Doe",
                EmployeeId = "1",
                SeatRequestId = 101,
                RequestDate = DateTime.Now.AddDays(-5),
                RequestFor = DateTime.Now.AddDays(2),
                Email = "john.doe@example.com",
                FloorNo = "Floor 1",
                DeskNo = "A1-01",
                Status = 1
            },
            new GetSeatRequestResponseModel
            {
                Name = "Jane Smith",
                EmployeeId = "2",
                SeatRequestId = 102,
                RequestDate = DateTime.Now.AddDays(-3),
                RequestFor = DateTime.Now.AddDays(1),
                Email = "jane.smith@example.com",
                FloorNo = "Floor 2",
                DeskNo = "B2-05",
                Status = 2
            }
        };
            _seatRequestRepository.Setup(x => x.GetAllSeatRequest(pageNo, pageSize, search, sort)).ReturnsAsync(requestResults);

            // Act
            var result = await _seatRequestServices.GetAllSeatRequest(pageNo, pageSize, search, sort);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Error);
            Assert.NotNull(result.Data);
            Assert.Equal(requestResults.Count, result.Data.Count);
        }

        [Fact]
        public async Task To_GetAllSeatRequest_WithInvalidSearch_ReturnsBadRequest()
        {
            // Arrange
            var pageNo = 1;
            var pageSize = 10;
            string search = "ab";
            string sort = "Pending";

            // Act
            var result = await _seatRequestServices.GetAllSeatRequest(pageNo, pageSize, search, sort);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Single(result.Error);
            Assert.Equal("Minimum three character required", result.Error[0]);
        }

        [Fact]
        public async Task To_GetSeatRequestById_ReturnsResponseDtoWithOkStatusCode()
        {
            // Arrange
            int seatRequestId = 1;

            var expectedSeatRequest = new GetSeatRequestByIdResponseModel()
            {
                Name = "John Doe",
                EmployeeId = "12345",
                Email = "john.doe@example.com",
                FloorNo = "Floor 1",
                SeatId = 5,
                RequestDate = DateTime.Now.Date,
                RequestedFor = DateTime.Now.Date.AddDays(5),
                DeskNo = "A3",
                Reason = "Test Reason"
            };

            _seatRequestRepository
            .Setup(x => x.GetSeatRequestById(seatRequestId))
            .ReturnsAsync(expectedSeatRequest);

            // Act
            var response = await _seatRequestServices.GetSeatRequestById(seatRequestId);

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.Equal("John Doe", response.Data.Name);
            Assert.Equal("12345", response.Data.EmployeeId);
            Assert.Equal("john.doe@example.com", response.Data.Email);
            Assert.Equal("Floor 1", response.Data.FloorNo);
            Assert.Equal(5, response.Data.SeatId);
            Assert.Equal(DateTime.Now.Date.ToString("dd-MM-yyyy"), response.Data.RequestDate);
            Assert.Equal(DateTime.Now.Date.AddDays(5).ToString("dd-MM-yyyy"), response.Data.RequestedFor);
            Assert.Equal("A3", response.Data.DeskNo);
            Assert.Equal("Test Reason", response.Data.Reason);
        }

        [Fact]

        public async Task To_GetSeatRequestById_ValidId_ReturnsResponseDtoWithBadRequestStatusCode()
        {
            // Arrange
            int seatRequestId = 1;

            _seatRequestRepository
                .Setup(x => x.GetSeatRequestById(seatRequestId))
                .ReturnsAsync((GetSeatRequestByIdResponseModel)null);

            // Act
            var response = await _seatRequestServices.GetSeatRequestById(seatRequestId);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(response.Data);
            Assert.Single(response.Error);
            Assert.Equal("SeatRequestId not Found", response.Error[0]);
        }

        [Fact]
        public async Task To_UpdateSeatRequest_WithApprovedStatus_ShouldUpdateRequestAndSendApprovalEmail()
        {
            // Arrange
            int seatRequestId = 1;
            string userRegisteredId = "user123";
            string employeeId = "123";
            DateTime bookingDate = DateTime.Parse("2023-06-15");
            var seatRequestDto = new UpdateUserSeatRequestDto
            {
                EmployeeId = userRegisteredId,
                RequestStatus = 2 // Approved status
            };
            var seatRequest = new SeatRequestModel
            {
                SeatRequestId = 1,
                EmployeeId = "123",
                BookingDate = DateTime.Parse("2023-06-15"),
                ModifiedDate = DateTime.Now,
                ModifiedBy = null,
                RequestStatus = 1
            };

            var rejectlist = new List<SeatRequestModel>
        {
            new SeatRequestModel
            {
                SeatRequestId = 1,
                EmployeeId = "123",
                SeatId = 2,
                CreatedDate = DateTime.Parse("2023-06-11"),
                BookingDate = DateTime.Parse("2023-06-15"),
                Reason = "Another request",
                RequestStatus = 1
            },
            new SeatRequestModel
            {
                SeatRequestId = 3,
                EmployeeId = userRegisteredId,
                SeatId = 3,
                CreatedDate = DateTime.Parse("2023-06-11"),
                BookingDate = DateTime.Parse("2023-06-15"),
                Reason = "Yet another request",
                RequestStatus = 1
            },
        };

            var objectList = new List<GetUserSeatRequestResponseModel>
    {
        new GetUserSeatRequestResponseModel
        {
            Name = "Jay Patl",
            Email = "jay@example.com",
            Floor = "5th Floor",
            RequestStatus = 1,
            BookingDate = DateTime.ParseExact("2023-06-15", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            SeatNumber = "A10",
            Location = "Surat",
            SeatRequestId = 2
        }
    };

            var seat = new GetSeatRequestByIdResponseModel
            {
                Name = "John Doe",
                EmployeeId = "12345",
                Email = "john.doe@example.com",
                FloorNo = "Floor 1",
                SeatId = 5,
                RequestDate = DateTime.Now.Date,
                RequestedFor = DateTime.Now.Date.AddDays(5),
                DeskNo = "A3",
                Reason = "Test Reason",
                City = "City Name",
                SeatNumber = "A3"
            };

            var userSeat = new List<GetUserSeatRequestResponseModel>
        {
            new GetUserSeatRequestResponseModel
            {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Floor = "Floor 1",
            RequestStatus = 1,
            BookingDate = DateTime.Now.Date,
            SeatNumber = "A3",
            Location = "City Name",
            SeatRequestId = 123
            }
        };


            _seatRequestRepository.Setup(r => r.GetUserSeatRequest(It.IsAny<int>())).ReturnsAsync(seatRequest);

            _seatRequestRepository.Setup(r => r.GetSeatRequestById(It.IsAny<int>())).ReturnsAsync(seat);

            _seatRequestRepository.Setup(r => r.GetMultipleSeatRequest(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(objectList);

            _seatRequestRepository.Setup(r => r.GetDisapprovedSeat(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(userSeat);

            _seatRequestRepository.Setup(r => r.ApproveEmail(It.IsAny<EmailResponseModel>())).Returns(Task.CompletedTask);

            _seatRequestRepository.Setup(r => r.RejectedMail(It.IsAny<EmailResponseModel>())).Returns(Task.CompletedTask);

            _seatRequestRepository.Setup(r => r.UpdateSeatRequestById(seatRequest)).Returns(Task.CompletedTask);

            _seatRequestRepository.Setup(r => r.UpdateSeatRequest(rejectlist)).Returns(Task.CompletedTask);


            // Act
            var response = await _seatRequestServices.UpdateSeatRequest(seatRequestId, seatRequestDto, employeeId);


            // Assert 
            Assert.Equal("Changes Have been Successful", response.Data);
            Assert.Null(response.Error);
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task To_UpdateSeatRequest_WithRejectStatus_ShouldUpdateRequestAndSendRejectEmail()
        {
            // Arrange
            int seatRequestId = 1;
            string userRegisteredId = "user123";
            string employeeId = "123";
            DateTime bookingDate = DateTime.Parse("2023-06-15");
            var seatRequestDto = new UpdateUserSeatRequestDto
            {
                EmployeeId = userRegisteredId,
                RequestStatus = 3
            };
            var seatRequest = new SeatRequestModel
            {
                SeatRequestId = 1,
                EmployeeId = "123",
                BookingDate = DateTime.Parse("2023-06-15"),
                ModifiedDate = DateTime.Now,
                ModifiedBy = null,
                RequestStatus = 1
            };

            var rejectlist = new List<SeatRequestModel>
        {
            new SeatRequestModel
            {
                SeatRequestId = 1,
                EmployeeId = "123",
                SeatId = 2,
                CreatedDate = DateTime.Parse("2023-06-11"),
                BookingDate = DateTime.Parse("2023-06-15"),
                Reason = "Another request",
                RequestStatus = 1
            },
            new SeatRequestModel
            {
                SeatRequestId = 3,
                EmployeeId = userRegisteredId,
                SeatId = 3,
                CreatedDate = DateTime.Parse("2023-06-11"),
                BookingDate = DateTime.Parse("2023-06-15"),
                Reason = "Yet another request",
                RequestStatus = 1
            },
        };

            var objectList = new List<GetUserSeatRequestResponseModel>
    {
        new GetUserSeatRequestResponseModel
        {
            Name = "Jay Patl",
            Email = "jay@example.com",
            Floor = "5th Floor",
            RequestStatus = 1,
            BookingDate = DateTime.ParseExact("2023-06-15", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            SeatNumber = "A10",
            Location = "Surat",
            SeatRequestId = 2
        }
    };

            var seat = new GetSeatRequestByIdResponseModel
            {
                Name = "John Doe",
                EmployeeId = "12345",
                Email = "john.doe@example.com",
                FloorNo = "Floor 1",
                SeatId = 5,
                RequestDate = DateTime.Now.Date,
                RequestedFor = DateTime.Now.Date.AddDays(5),
                DeskNo = "A3",
                Reason = "Test Reason",
                City = "City Name",
                SeatNumber = "A3"
            };

            var userSeat = new List<GetUserSeatRequestResponseModel>
        {
            new GetUserSeatRequestResponseModel
            {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Floor = "Floor 1",
            RequestStatus = 1,
            BookingDate = DateTime.Now.Date,
            SeatNumber = "A3",
            Location = "City Name",
            SeatRequestId = 123
            }
        };

            _seatRequestRepository.Setup(r => r.GetUserSeatRequest(It.IsAny<int>())).ReturnsAsync(seatRequest);

            _seatRequestRepository.Setup(r => r.GetSeatRequestById(It.IsAny<int>())).ReturnsAsync(seat);

            _seatRequestRepository.Setup(r => r.GetMultipleSeatRequest(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(objectList);

            _seatRequestRepository.Setup(r => r.GetDisapprovedSeat(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(userSeat);

            _seatRequestRepository.Setup(r => r.ApproveEmail(It.IsAny<EmailResponseModel>())).Returns(Task.CompletedTask);

            _seatRequestRepository.Setup(r => r.RejectedMail(It.IsAny<EmailResponseModel>())).Returns(Task.CompletedTask);

            _seatRequestRepository.Setup(r => r.UpdateSeatRequestById(seatRequest)).Returns(Task.CompletedTask);

            _seatRequestRepository.Setup(r => r.UpdateSeatRequest(rejectlist)).Returns(Task.CompletedTask);

            // Act
            var response = await _seatRequestServices.UpdateSeatRequest(seatRequestId, seatRequestDto, employeeId);

            // Assert 
            Assert.Equal("Changes Have been Successful", response.Data);
            Assert.Null(response.Error);
            Assert.Equal(200, response.StatusCode);
        }
    }
}