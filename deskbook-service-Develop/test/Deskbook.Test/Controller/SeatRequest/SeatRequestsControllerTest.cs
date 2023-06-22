using System.Net;
using DeskBook.API.Controllers;
using DeskBook.AppServices.Contracts.SeatRequest;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.SeatRequest;
using DeskBook.AppServices.Services.SeatRequest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.SeatRequest
{
    public class SeatRequestsControllerTest
    {
        private readonly Mock<IUserSeatRequestServices> _userSeatRequestService;
        private readonly UserSeatRequestsController _userSeatRequestController;
        private readonly Mock<ILogger<UserSeatRequestsController>> _logger;

        public SeatRequestsControllerTest()
        {
            _userSeatRequestService = new Mock<IUserSeatRequestServices>();
            _logger = new Mock<ILogger<UserSeatRequestsController>>();
            _userSeatRequestController = new UserSeatRequestsController(_userSeatRequestService.Object, _logger.Object);
        }

        [Fact]
        public async Task To_Check_GetAllSeatRequest_ReturnsOkResultWithData()
        {
            // Arrange
            var search = "John";
            var sort = "Pending";
            var pageNo = 1;
            var pageSize = 10;

            var seatRequests = new List<GetSeatRequestResultsDto>
        {
            new GetSeatRequestResultsDto
            {
                Name = "John Doe",
                EmployeeId = "1",
                SeatRequestId = 101,
                RequestDate = "01-06-2023",
                RequestFor = "05-06-2023",
                Email = "john.doe@example.com",
                FloorNo = "Floor 1",
                DeskNo = "A1-01",
                Status = 1
            },
        };

            var response = new ResponseDto<List<GetSeatRequestResultsDto>>
            {
                Data = seatRequests,
                Error = null,
                StatusCode = (int)HttpStatusCode.OK
            };

            _userSeatRequestService.Setup(x => x.GetAllSeatRequest(pageNo, pageSize, search, sort)).ReturnsAsync(response);

            // Act
            var result = await _userSeatRequestController.GetAllSeatRequest(pageNo, pageSize, search, sort);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var data = Assert.IsType<ResponseDto<List<GetSeatRequestResultsDto>>>(okResult.Value);
            Assert.Equal(response.Data, data.Data);
        }

        [Fact]
        public async Task To_Check_GetAllSeatRequest_ReturnsBadRequest()
        {
            // Arrange
            var search = "Jo";
            var sort = "Pending";
            var pageNo = 1;
            var pageSize = 10;

            var badRequestResponse = new ResponseDto<List<GetSeatRequestResultsDto>>()
            {
                Data = null,
                Error = new List<string> { "Minimum three characters required" },
                StatusCode = (int)HttpStatusCode.BadRequest
            };
            _userSeatRequestService.Setup(x => x.GetAllSeatRequest(pageNo, pageSize, search, sort)).ReturnsAsync(badRequestResponse);

            // Act
            var result = await _userSeatRequestController.GetAllSeatRequest(pageNo, pageSize, search, sort);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
            Assert.Equal(badRequestResponse, badRequestResult.Value);
        }

        [Fact]
        public async Task To_Check_GetAllSeatRequest_ReturnsInternalServerError()
        {
            // Arrange
            var search = "John";
            var sort = "Pending";
            var pageNo = 1;
            var pageSize = 10;

            ResponseDto<List<GetSeatRequestResultsDto>> nullResponse = null;
            _userSeatRequestService.Setup(x => x.GetAllSeatRequest(pageNo, pageSize, search, sort)).ReturnsAsync(nullResponse);

            // Act
            var result = await _userSeatRequestController.GetAllSeatRequest(pageNo, pageSize, search, sort);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
            Assert.Null(objectResult.Value);
        }

        [Fact]
        public async Task To_Check_GetSeatRequestById_ReturnsResponseDtoWithOkStatusCode()
        {
            // Arrange
            int seatRequestId = 1;
            var expectedData = new GetSeatRequestByIdResponseDto
            {
                Name = "John Doe",
                EmployeeId = "12345",
                Email = "john.doe@example.com",
                FloorNo = "Floor 1",
                SeatId = 5,
                RequestDate = DateTime.Now.Date.ToString("dd-MM-yyyy"),
                RequestedFor = DateTime.Now.Date.AddDays(5).ToString("dd-MM-yyyy"),
                DeskNo = "A3",
                Reason = "Test Reason"
            };
            var expectedResponse = new ResponseDto<GetSeatRequestByIdResponseDto>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = expectedData
            };
            _userSeatRequestService.Setup(x => x.GetSeatRequestById(seatRequestId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userSeatRequestController.GetSeatRequestById(seatRequestId);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var response = Assert.IsType<ResponseDto<GetSeatRequestByIdResponseDto>>(okResult.Value);
            Assert.Equal(expectedResponse.Data, response.Data);
        }

        [Fact]
        public async Task To_Check_GetSeatRequestById_ReturnsResponseDtoWithBadRequestStatusCode()
        {
            // Arrange
            int seatRequestId = 1;
            var expectedResponse = new ResponseDto<GetSeatRequestByIdResponseDto>()
            {
                Data = null,
                Error = new List<string> { "SeatRequestId not Found" },
                StatusCode = (int)HttpStatusCode.BadRequest
            };

            _userSeatRequestService.Setup(x => x.GetSeatRequestById(seatRequestId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userSeatRequestController.GetSeatRequestById(seatRequestId);

            // Assert
            Assert.NotNull(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
            var response = Assert.IsType<ResponseDto<GetSeatRequestByIdResponseDto>>(badRequestResult.Value);
            Assert.Null(response.Data);
            Assert.Equal(expectedResponse.Error, response.Error);
        }

        [Fact]
        public async Task To_Check_GetSeatRequestById_ReturnsResponseDtoWithInternalServerErrorStatusCode()
        {
            // Arrange
            int seatRequestId = 1;
            var expectedResponse = new ResponseDto<GetSeatRequestByIdResponseDto>()
            {
                Data = null,
                Error = null,
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            _userSeatRequestService.Setup(x => x.GetSeatRequestById(seatRequestId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userSeatRequestController.GetSeatRequestById(seatRequestId);

            // Assert
            Assert.NotNull(result);
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
            var response = Assert.IsType<ResponseDto<GetSeatRequestByIdResponseDto>>(internalServerErrorResult.Value);
            Assert.Null(response.Data);
            Assert.Null(response.Error);
        }

        [Fact]
        public async Task To_Check_UpdateSeatRequest_ValidData_ReturnsOkResult()
        {
            // Arrange
            int seatRequestId = 1;
            var seatRequestDto = new UpdateUserSeatRequestDto
            {
                EmployeeId = "1",
                RequestStatus = 2
            };
            var expectedResult = new ResponseDto<string>
            {
                Data = "Changes Have been Successful",
                Error = null,
                StatusCode = (int)HttpStatusCode.OK
            };
            _userSeatRequestService.Setup(s => s.UpdateSeatRequest(seatRequestId, seatRequestDto, It.IsAny<string>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _userSeatRequestController.UpdateSeatRequest(seatRequestId, seatRequestDto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedResult, result.Value);
        }

        [Fact]
        public async Task To_Check_UpdateSeatRequest_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            int seatRequestId = 1;
            var seatRequestDto = new UpdateUserSeatRequestDto
            {
                EmployeeId = "1",
                RequestStatus = 5
            };
            var expectedResult = new ResponseDto<string>
            {
                Data = null,
                Error = null,
                StatusCode = (int)HttpStatusCode.BadRequest
            };
            _userSeatRequestService.Setup(s => s.UpdateSeatRequest(seatRequestId, seatRequestDto, It.IsAny<string>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _userSeatRequestController.UpdateSeatRequest(seatRequestId, seatRequestDto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(expectedResult, result.Value);
        }

        [Fact]
        public async Task To_Check_UpdateSeatRequest_InternalServerError_ReturnsInternalServerError()
        {
            // Arrange
            int seatRequestId = 1;
            var seatRequestDto = new UpdateUserSeatRequestDto
            {
                EmployeeId = "1",
                RequestStatus = 2
            };
            var expectedResult = new ResponseDto<string>
            {
                Data = null,
                Error = null,
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            _userSeatRequestService.Setup(s => s.UpdateSeatRequest(seatRequestId, seatRequestDto, It.IsAny<string>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _userSeatRequestController.UpdateSeatRequest(seatRequestId, seatRequestDto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(expectedResult, result.Value);
        }
    }
}