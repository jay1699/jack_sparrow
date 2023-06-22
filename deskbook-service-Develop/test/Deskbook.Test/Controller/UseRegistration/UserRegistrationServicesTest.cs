using System.Net;
using DeskBook.AppServices.Contracts.AuthorityRegistration;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.UserRegistration;
using DeskBook.AppServices.Services.UserRegistrationServices;
using DeskBook.Infrastructure.Contracts.ITokenRepository;
using DeskBook.Infrastructure.Contracts.Seat;
using DeskBook.Infrastructure.Contracts.UserRegistration;
using DeskBook.Infrastructure.Model.AuthoritySetting;
using DeskBook.Infrastructure.Model.Seat;
using DeskBook.Infrastructure.Model.UserRegistration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.UserRegistration
{
    public class UserRegistrationServicesTest
    {

        private readonly UserRegistrationServices _userRegistration;

        private readonly Mock<ILogger<UserRegistrationServices>> _logger;

        private readonly Mock<IAuthorityRegistrationServices> _authorityservices;

        private readonly Mock<AuthorityModel> _authoritySettings;

        private readonly Mock<IUserRegistrationRepository> _userRepository;

        private readonly Mock<ISeatRepository> _seatRepository;

        public UserRegistrationServicesTest()
        {
            _logger = new Mock<ILogger<UserRegistrationServices>>();
            _userRepository = new Mock<IUserRegistrationRepository>();
            _authorityservices = new Mock<IAuthorityRegistrationServices>();
            _authoritySettings = new Mock<AuthorityModel>();
            _seatRepository = new Mock<ISeatRepository>();
            _authoritySettings = new Mock<AuthorityModel>();
            _userRegistration = new UserRegistrationServices(_logger.Object, _userRepository.Object, _authorityservices.Object, _seatRepository.Object, _authoritySettings.Object);
        }

        [Fact]
        public async Task To_RegisterUserWithAuthority_WhenUserRegistrationIsSuccessful_ReturnsResponseDtoWithOkStatusCode()
        {
            // Arrange
            var userRequest = new UserRequestDto
            {
                Email = "jay.ravan@example.com",
                Password = "Password123",
                FirstName = "Jay",
                LastName = "Patel"
            };

            _userRepository.Setup(r => r.GetByEmail(userRequest.Email)).ReturnsAsync((string)null);
            _authorityservices.Setup(a => a.RegisterUser(It.IsAny<UserRegistrationDto>())).ReturnsAsync(new ResponseDto<UserResponse>
            {
                Data = new UserResponse()
                {
                    StatusCode = 200,
                    Responses = "{subjectid:1}"
                },
                StatusCode = 200,
            });

            // Act
            var result = await _userRegistration.RegisterUserWithAuthority(userRequest);

            // Assert
            Assert.NotNull(result);
            var responseDto = Assert.IsType<ResponseDto<string>>(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)responseDto.StatusCode);
        }

        [Fact]
        public async Task To_RegisterUserWithAuthority_WhenUserRegistrationFails_ReturnsResponseDtoWithBadRequest()
        {
            // Arrange
            var userRequest = new UserRequestDto
            {
                Email = "jay.ravan@example.com",
                Password = "Password123",
                FirstName = "Jay",
                LastName = "Patel"
            };
            _userRepository.Setup(r => r.GetByEmail(userRequest.Email)).ReturnsAsync((string)null);
            _authorityservices.Setup(a => a.RegisterUser(It.IsAny<UserRegistrationDto>())).ReturnsAsync(new ResponseDto<UserResponse>
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            });

            // Act
            var result = await _userRegistration.RegisterUserWithAuthority(userRequest);

            // Assert
            var responseDto = Assert.IsType<ResponseDto<string>>(result);
            Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)responseDto.StatusCode);
        }

        [Fact]
        public async void To_RegisterUserWithAuthority_WhenEmailIdExists_ReturnsResponseDtoWithBadRequest()
        {
            // Arrange
            var userRequest = new UserRequestDto
            {
                Email = "jay.ravan@example.com",
                Password = "Password123",
                FirstName = "Jay",
                LastName = "Patel"
            };

            _userRepository.Setup(r => r.GetByEmail(userRequest.Email)).ReturnsAsync("jay.ravan@example.com");

            // Act
            var result = await _userRegistration.RegisterUserWithAuthority(userRequest);

            // Assert
            var responseDto = Assert.IsType<ResponseDto<string>>(result);
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)responseDto.StatusCode);
        }

        [Fact]
        public async Task To_GetUsers_ReturnsListOfRegisteredUsers()
        {
            // Arrange
            int pageNo = 1;
            int pageSize = 10;
            string search = "megha";

            var users = new List<GetRegistredUserResponseModel>
            {
                new GetRegistredUserResponseModel { EmployeeId = "1", EmailId = "megha@gmail.com", Name = "Megha Patel", IsActive = true },
                new GetRegistredUserResponseModel { EmployeeId = "2", EmailId = "meghaT123@gmail.com", Name = "Megha Tandel", IsActive = true }
            };

            _userRepository.Setup(r => r.GetUsers(pageNo, pageSize, search)).ReturnsAsync(users);

            // Act
            var result = await _userRegistration.GetUsers(pageNo, pageSize, search);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);

            var firstUser = result.Data[0];
            Assert.Equal("1", firstUser.EmployeeId);
            Assert.Equal("Megha Patel", firstUser.Name);
            Assert.Equal("megha@gmail.com", firstUser.Email);
            Assert.True(firstUser.Status);
            Assert.Null(firstUser.Designation);

            var secondUser = result.Data[1];
            Assert.Equal("2", secondUser.EmployeeId);
            Assert.Equal("Megha Tandel", secondUser.Name);
            Assert.Equal("meghaT123@gmail.com", secondUser.Email);
            Assert.True(secondUser.Status);
            Assert.Null(secondUser.Designation);
        }

        [Fact]
        public async Task To_UpdateUserStatus_WhenCalledWithValidData_ReturnsSuccessResponse()
        {
            // Arrange
            string employeeId = "1";
            var userStatusDto = new List<UpdateUserRequestDto>
        {
            new UpdateUserRequestDto
            {
                EmployeeId = "123",
                IsActive = true
            }
        };

            var userRegistrationModel = new UserRegistrationModel
            {
                EmployeeId = "123",
                IsActive = true,
            };

            var seatConfigurationModel = new SeatConfigurationModel
            {
                EmployeeId = "123",
            };

            _userRepository
                .Setup(x => x.GetEmployee("123"))
                .ReturnsAsync(userRegistrationModel);

            _seatRepository
                .Setup(x => x.GetBySeats("123"))
                .ReturnsAsync(seatConfigurationModel);

            // Act
            var response = await _userRegistration.UpdateUserStatus(userStatusDto, employeeId);

            // Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("The Changes have been Successfully Saved", response.Data);
        }

        [Fact]
        public async Task To_UpdateUserStatus_WhenUserNotFound_ReturnsBadRequestResponse()
        {
            // Arrange
            string employeeId = "1";
            var userStatusDto = new List<UpdateUserRequestDto>
        {
            new UpdateUserRequestDto
            {
                EmployeeId = "1",
                IsActive = false
            }
        };

            _userRepository
                .Setup(x => x.GetById("1"))
                .ReturnsAsync((UserRegistrationModel)null);

            // Act
            var response = await _userRegistration.UpdateUserStatus(userStatusDto, employeeId);

            // Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(response.Data);
        }

        [Fact]
        public async Task To_GetEmployeeById_ReturnsResponseDtoWithOkStatusCode()
        {
            // Arrange
            string employeeId = "12345";

            _userRepository.Setup(x => x.GetEmployeeById(employeeId)).ReturnsAsync(new List<GetUserResponseModel>()
                {
                new GetUserResponseModel()
                {
                    ProfilePictureFilePath = null,
                    EmailId = "test@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "1234567890",
                    DesignationId = 1,
                    DesignationName = "Manager",
                    ModeOfWorkId = 1,
                    ModeOfWork = "Regular",
                    CityId = 1,
                    CityName = "Valsad",
                    FloorId = 1,
                    FloorName = "1st Floor",
                    ColumnId = 1,
                    ColumnName = "A",
                    SeatId = 1,
                    SeatNumber = 12,
                    WorkingDayId = 1,
                    WorkingDay = "Monday",
                }
            });

            // Act
            var response = await _userRegistration.GetEmployeeById(employeeId);

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.Equal("test@example.com", response.Data.EmailId);
            Assert.Equal("John", response.Data.FirstName);
            Assert.Equal("Doe", response.Data.LastName);
            Assert.Equal("1234567890", response.Data.PhoneNumber);

            Assert.NotNull(response.Data.designation);
            Assert.Equal(1, response.Data.designation.Id);
            Assert.Equal("Manager", response.Data.designation.Name);

            Assert.NotNull(response.Data.modeOfWork);
            Assert.Equal(1, response.Data.modeOfWork.Id);
            Assert.Equal("Regular", response.Data.modeOfWork.Name);

            Assert.NotNull(response.Data.city);
            Assert.Equal(1, response.Data.city.Id);
            Assert.Equal("Valsad", response.Data.city.Name);

            Assert.NotNull(response.Data.floor);
            Assert.Equal(1, response.Data.floor.Id);
            Assert.Equal("1st Floor", response.Data.floor.Name);

            Assert.NotNull(response.Data.column);
            Assert.Equal(1, response.Data.column.Id);
            Assert.Equal("A", response.Data.column.Name);

            Assert.NotNull(response.Data.seat);
            Assert.Equal(1, response.Data.seat.Id);
            Assert.Equal((byte)12, response.Data.seat.SeatNumber);

            Assert.NotNull(response.Data.days);
            Assert.Equal(1, response.Data.days.Count);
            Assert.Equal("Monday", response.Data.days[0].Day);
            Assert.Equal((byte)1, response.Data.days[0].Id);
        }
    }
}