using System.Net;
using DeskBook.API.Controllers;
using DeskBook.AppServices.Contracts.UserRegistration;
using DeskBook.AppServices.DTOs.RegisteredUser;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.UserRegistration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeskBook.Test.Controller.UserRegistration
{
    public class UserControllerTest
    {
        private readonly Mock<IUserRegistrationServices> _userRegistrationServices;
        private readonly UsersController _userController;
        private readonly Mock<ILogger<UsersController>> _logger;

        public UserControllerTest()
        {
            _userRegistrationServices = new Mock<IUserRegistrationServices>();
            _logger = new Mock<ILogger<UsersController>>();
            _userController = new UsersController(_userRegistrationServices.Object, _logger.Object);
        }

        [Fact]
        public async Task To_Check_AddUser_WhenUserDoesNotExist_ShouldReturnOk()
        {
            // Arrange
            var userRequestDto = new UserRequestDto
            {
                Email = "Kaml@yoqmail.com",
                Password = "Hjs@1234",
                FirstName = "Kamala",
                LastName = "Tewatiya"
            };

            // Act
            var result = await _userController.AddUser(userRequestDto);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(okObjectResult.StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task To_Check_AddUser_WhenModelStateisInvalid_ShouldReturnBadRequest()
        {
            // Arrange
            var userRequestDto = new UserRequestDto
            {
                Email = "",
                Password = "",
                FirstName = "",
                LastName = ""
            };

            _userController.ModelState.AddModelError("Email", "The Email field is required.");
            _userController.ModelState.AddModelError("Password", "The Password field is required.");
            _userController.ModelState.AddModelError("FirstName", "The FirstName field is required.");
            _userController.ModelState.AddModelError("LastName", "The LastName field is required.");

            // Act
            var result = await _userController.AddUser(userRequestDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(badRequestResult.StatusCode == (int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task To_Check_AddUser_WhenAddUserRegistrationFails_ShouldReturnStatusCode500()
        {
            // Arrange
            var userRequestDto = new UserRequestDto
            {
                Email = "Kaml@yoqmail.com",
                Password = "Hjs@1234",
                FirstName = "Kamala",
                LastName = "Tewatiya"
            };

            _userRegistrationServices
                .Setup(x => x.RegisterUserWithAuthority(userRequestDto))
                .ReturnsAsync(new ResponseDto<string>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Error = new List<string>
                {
                    { "An error occurred while registering the user." }
                }
                });

            // Act
            var result = await _userController.AddUser(userRequestDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task To_Check_AddUser_WhenUserRegistrationReturnsBadRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var userRequestDto = new UserRequestDto
            {
                Email = "Kaml@yoqmail.com",
                Password = "Hjs@1234",
                FirstName = "Kamala",
                LastName = "Tewatiya"
            };

            var expectedErrors = new List<string>
        {
                {"Email Id Already Exists" }
        };

            var expectedResponse = new ResponseDto<string>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Error = expectedErrors
            };

            _userRegistrationServices
                .Setup(x => x.RegisterUserWithAuthority(userRequestDto))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userController.AddUser(userRequestDto);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expectedResponse.StatusCode, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task To_Check_UpdateUserStatus_WithValidData_ShouldReturnSuccessResponse()
        {
            // Arrange
            var request = new List<UpdateUserRequestDto>
        {
            new UpdateUserRequestDto
            {
                IsActive = true,
                DeletedDate= DateTime.Now
            }
        };

            _userRegistrationServices
               .Setup(x => x.UpdateUserStatus(request, It.IsAny<string>()))
               .ReturnsAsync(new ResponseDto<string>
               {
                   StatusCode = (int)HttpStatusCode.OK,
                   Data = "The Changes have been Successfully Saved"
               });

            // Act
            var response = await _userController.UpdateUserStatus(request);

            // Assert
            Assert.NotNull(response);
            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task To_Check_UpdateUserStatus_WithValidData_ShouldReturnBadRequestResponse()
        {
            // Arrange
            var request = new List<UpdateUserRequestDto>
        {
            new UpdateUserRequestDto
            {
                IsActive = true,
                DeletedDate= DateTime.Now
            }
        };

            var expectedErrorMessage = "Employee Id not Found";
            _userRegistrationServices
               .Setup(x => x.UpdateUserStatus(request, It.IsAny<string>()))
               .ReturnsAsync(new ResponseDto<string>
               {
                   StatusCode = (int)HttpStatusCode.BadRequest,
                   Error = new List<string> { expectedErrorMessage }
               });

            // Act
            var result = await _userController.UpdateUserStatus(request);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestObjectResult.StatusCode);
            var responseDto = Assert.IsType<ResponseDto<string>>(badRequestObjectResult.Value);
            Assert.Equal(expectedErrorMessage, responseDto.Error.First());
        }

        [Fact]
        public async Task To_Check_UpdateUserStatus_WhenUpdateUserStatusReturnNull_ShouldReturnIntenalServerError()
        {
            // Arrange
            var request = new List<UpdateUserRequestDto>
        {
            new UpdateUserRequestDto
            {
                IsActive = true,
                DeletedDate= DateTime.Now
            }
        };

            _userRegistrationServices
               .Setup(x => x.UpdateUserStatus(request, It.IsAny<string>()))
               .ReturnsAsync((ResponseDto<string>)null);

            // Act
            var result = await _userController.UpdateUserStatus(request);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetUsers_ValidInput_ReturnsOk()
        {
            // Arrange
            var pageNo = 1;
            var pageSize = 10;
            var search = "Megha";

            var expectedResult = new ResponseDto<List<GetRegisteredUserResultDto>>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = new List<GetRegisteredUserResultDto>
            {
                 new GetRegisteredUserResultDto { EmployeeId = "1", Email = "megha@gmail.com", Name = "Megha Patel", Designation = "Developer", Status = true },
                 new GetRegisteredUserResultDto { EmployeeId = "2", Email = "meghaT123@gmail.com", Name = "Megha Tandel", Designation = "Developer", Status = true }
            }
            };
            _userRegistrationServices.Setup(s => s.GetUsers(pageNo, pageSize, search)).ReturnsAsync(expectedResult);

            // Act
            var result = await _userController.GetUsers(pageNo, pageSize, search);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.Same(expectedResult, objectResult.Value);
        }

        [Fact]
        public async Task GetUsers_ReturnsBadRequestResult()
        {
            // Arrange
            int pageNo = 1;
            int pageSize = 10;
            string search = "megha";

            var response = new ResponseDto<List<GetRegisteredUserResultDto>>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Error = new List<string>
            {
                    { "An error occurred while Getting the user." }
            }
            };

            _userRegistrationServices.Setup(s => s.GetUsers(pageNo, pageSize, search)).ReturnsAsync(response);

            // Act
            var result = await _userController.GetUsers(pageNo, pageSize, search);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var data = Assert.IsType<ResponseDto<List<GetRegisteredUserResultDto>>>(badRequestResult.Value);

            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
            Assert.Equal("An error occurred while Getting the user.", data.Error.First());
        }

        [Fact]
        public async Task GetUsers_ReturnsInternalServerErrorResult()
        {
            // Arrange
            int pageNo = 1;
            int pageSize = 10;
            string search = "megha";

            var response = new ResponseDto<List<GetRegisteredUserResultDto>>
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Error = new List<string>
            {
                    { "Internal Server Error." }
            }
            };
            _userRegistrationServices.Setup(s => s.GetUsers(pageNo, pageSize, search)).ReturnsAsync(response);

            // Act
            var result = await _userController.GetUsers(pageNo, pageSize, search);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal(response, internalServerErrorResult.Value);
        }

        [Fact]
        public async Task To_Check_GetEmployeeById_ReturnsOkResultWithData()
        {
            // Arrange
            string employeeId = "12345";
            var expectedData = new GetUserResultsDto
            {
                EmailId = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                ProfilePictureFileString = null,
                designation = new UserCommonDto
                {
                    Id = 1,
                    Name = "Manager"
                },
                days = new List<GetUserDayDto>
            {
                new GetUserDayDto
                {
                    Id = 1,
                    Day = "Monday"
                }
            },
                modeOfWork = new UserCommonDto
                {
                    Id = 1,
                    Name = "Regular"
                },
                city = new UserCommonDto
                {
                    Id = 1,
                    Name = "Valsad"
                },
                floor = new UserCommonDto
                {
                    Id = 1,
                    Name = "1st Floor"
                },
                column = new UserCommonDto
                {
                    Id = 1,
                    Name = "A"
                },
                seat = new GetSeatUserDto
                {
                    Id = 1,
                    SeatNumber = 12
                }
            };
            var expectedResponse = new ResponseDto<GetUserResultsDto>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = expectedData
            };
            _userRegistrationServices.Setup(x => x.GetEmployeeById(employeeId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userController.GetEmployeeById(employeeId);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var response = Assert.IsType<ResponseDto<GetUserResultsDto>>(okResult.Value);
            Assert.Equal(expectedResponse.Data, response.Data);
        }

        [Fact]
        public async Task To_Check_GetEmployeeById_ReturnBadRequestResult()
        {
            // Arrange
            string employeeId = "12345";
            var expectedData = new GetUserResultsDto
            {
                EmailId = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                ProfilePictureFileString = null,
                designation = new UserCommonDto
                {
                    Id = 1,
                    Name = "Manager"
                },
                days = new List<GetUserDayDto>
            {
                new GetUserDayDto
                {
                    Id = 1,
                    Day = "Monday"
                }
            },
                modeOfWork = new UserCommonDto
                {
                    Id = 1,
                    Name = "Regular"
                },
                city = new UserCommonDto
                {
                    Id = 1,
                    Name = "Valsad"
                },
                floor = new UserCommonDto
                {
                    Id = 1,
                    Name = "1st Floor"
                },
                column = new UserCommonDto
                {
                    Id = 1,
                    Name = "A"
                },
                seat = new GetSeatUserDto
                {
                    Id = 1,
                    SeatNumber = 12
                }
            };
            var expectedResponse = new ResponseDto<GetUserResultsDto>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Error = new List<string>
                {
                    {"An error occurred while Getting Employee By Id" }
                }
            };
            _userRegistrationServices.Setup(x => x.GetEmployeeById(employeeId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userController.GetEmployeeById(employeeId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
            var response = Assert.IsType<ResponseDto<GetUserResultsDto>>(badRequestResult.Value);
            Assert.Equal("An error occurred while Getting Employee By Id", response.Error.First());
        }

        [Fact]
        public async Task To_Check_GetEmployeeById_ReturnInternalServerError()
        {
            // Arrange
            string employeeId = "12345";
            var expectedData = new GetUserResultsDto
            {
                EmailId = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                ProfilePictureFileString = null,
                designation = new UserCommonDto
                {
                    Id = 1,
                    Name = "Manager"
                },
                days = new List<GetUserDayDto>
            {
                new GetUserDayDto
                {
                    Id = 1,
                    Day = "Monday"
                }
            },
                modeOfWork = new UserCommonDto
                {
                    Id = 1,
                    Name = "Regular"
                },
                city = new UserCommonDto
                {
                    Id = 1,
                    Name = "Valsad"
                },
                floor = new UserCommonDto
                {
                    Id = 1,
                    Name = "1st Floor"
                },
                column = new UserCommonDto
                {
                    Id = 1,
                    Name = "A"
                },
                seat = new GetSeatUserDto
                {
                    Id = 1,
                    SeatNumber = 12
                }
            };
            var expectedResponse = new ResponseDto<GetUserResultsDto>
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Error = new List<string>
                {
                    {"An error occurred while Getting Employee By Id" }
                }
            };
            _userRegistrationServices.Setup(x => x.GetEmployeeById(employeeId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userController.GetEmployeeById(employeeId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal(expectedResponse, internalServerErrorResult.Value);
        }
    }
}