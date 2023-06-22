using DeskBook.AppServices.Contracts.AuthorityRegistration;
using DeskBook.AppServices.Contracts.UserRegistration;
using DeskBook.AppServices.DTOs.RegisteredUser;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.UserRegistration;
using DeskBook.AppServices.Extension.ResponseExtention;
using DeskBook.Infrastructure.Contracts.Seat;
using DeskBook.Infrastructure.Contracts.UserRegistration;
using DeskBook.Infrastructure.Model.AuthoritySetting;
using DeskBook.Infrastructure.Model.Seat;
using DeskBook.Infrastructure.Model.UserRegistration;
using DeskBook.Infrastructure.Resource;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace DeskBook.AppServices.Services.UserRegistrationServices
{
    public class UserRegistrationServices : IUserRegistrationServices
    {
        private readonly IAuthorityRegistrationServices _authorityRegistration;

        private readonly AuthorityModel _authoritySettings;

        private readonly IUserRegistrationRepository _repository;

        private readonly ISeatRepository _seatRepository;

        private readonly ILogger<UserRegistrationServices> _logger;

        public UserRegistrationServices(ILogger<UserRegistrationServices> logger, IUserRegistrationRepository repository, IAuthorityRegistrationServices authorityRegistration, ISeatRepository seatRepository, AuthorityModel authoritySettings)
        {
            _logger = logger;
            _repository = repository;
            _authorityRegistration = authorityRegistration;
            _seatRepository = seatRepository;
            _authoritySettings = authoritySettings;
        }

        private async Task<ResponseDto<string>> AddUser(UserRegistrationDto userRegistration, UserRequestDto userRequest, ResponseDto<UserResponse> response)
        {
            var userResponse = response.Data;
            if (response != null && response.StatusCode == (int)HttpStatusCode.OK)
            {
                var responsess = JsonConvert.DeserializeObject<dynamic>(userResponse.Responses);
                var user = new UserRegistrationModel()
                {
                    EmployeeId = responsess.subjectId,
                    FirstName = userRequest.FirstName,
                    LastName = userRequest.LastName,
                    EmailId = userRequest.Email,
                    CreatedDate = DateTime.Now,
                    CreatedBy = responsess.subjectId,
                    IsActive = true
                };
                await _repository.AddUser(user);

                _logger.LogInformation("User registration Successful");
                var responseDto = new ResponseDto<string>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = ResponseMessage.RegistrationSuccessful
                };
                return responseDto;
            }
            else
            {
                _logger.LogInformation("User registration Failed");
                var responseDto = ResponseExtensions.ErrorResponse<string>(null, ResponseMessage.RegistrattionFailed, HttpStatusCode.InternalServerError);
                return responseDto;
            }
        }

        public async Task<ResponseDto<string>> RegisterUserWithAuthority(UserRequestDto userRequest)
        {
            var email = userRequest.Email;
            var emailID = await _repository.GetByEmail(email);
            if (!string.IsNullOrEmpty(emailID))
            {
                _logger.LogError("Email Id already Exist");
                var responseDto = ResponseExtensions.ErrorResponse<string>(null, ResponseMessage.EmailExist, HttpStatusCode.BadRequest);
                return responseDto;
            }

            var userRegistration = new UserRegistrationDto()
            {
                FullName = $"{userRequest.FirstName}",
                Email = userRequest.Email,
                UserName = userRequest.Email,
                PasswordHash = userRequest.Password,
                UserClient = _authoritySettings.ClientId,
                Roles = new List<RolesDto>()
            {
                new RolesDto {Name = "Employee"}
            },
                Claims = new List<ClaimsDto>()
            };

            var response = await _authorityRegistration.RegisterUser(userRegistration);
            var addUserResponse = await AddUser(userRegistration, userRequest, response);
            return addUserResponse;
        }

        public async Task<ResponseDto<List<GetRegisteredUserResultDto>>> GetUsers(int pageNo, int pageSize, string search)
        {
            if (search != null && search.Length < 2)
            {
                var response = new ResponseDto<List<GetRegisteredUserResultDto>>()
                {
                    Data = null,
                    Error = new List<string> { ResponseMessage.MinCharacter },
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return response;
            }

            var users = await _repository.GetUsers(pageNo, pageSize, search);

            var registeredUsers = new List<GetRegisteredUserResultDto>();
            foreach (var user in users)
            {
                var result = new GetRegisteredUserResultDto();
                result.EmployeeId = user.EmployeeId;
                result.Name = user.Name;
                result.Email = user.EmailId;
                if (user.DesignationName == null)
                {
                    result.Designation = null;
                }
                if (user.DesignationName != null)
                {
                    result.Designation = user.DesignationName;
                }
                result.Status = user.IsActive;
                registeredUsers.Add(result);
            }
            
            var responseDto = new ResponseDto<List<GetRegisteredUserResultDto>>()
            {
                Data = registeredUsers,
                Error = null,
                StatusCode = (int)HttpStatusCode.OK
            };
            _logger.LogInformation("All Users are retrieved successfully.");
            return responseDto;
        }

        public async Task<ResponseDto<string>> UpdateUserStatus(List<UpdateUserRequestDto> userStatusDto, string employeeId)
        {
            var userRegistrationModels = new List<UserRegistrationModel>();
            var seatConfigurationModels = new List<SeatConfigurationModel>();
            foreach (var user in userStatusDto)
            {
                if (!user.IsActive)
                {
                    var userresults = await _repository.GetById(user.EmployeeId);
                    var seats = await _seatRepository.GetBySeats(user.EmployeeId);
                    if (userresults != null)
                    {
                        userresults.PhoneNumber = null;
                        userresults.ModeOfWorkId = null;
                        userresults.ProfilePictureFileName = null;
                        userresults.ProfilePictureFilePath = null;
                        userresults.ModifiedDate = DateTime.Now;
                        userresults.ModifiedBy = employeeId;
                        userresults.DeletedDate = DateTime.Now;
                        userresults.DeletedBy = employeeId;
                        userresults.IsActive = user.IsActive;
                        userresults.DesignationId = null;
                        userRegistrationModels.Add(userresults);
                        _logger.LogInformation("The Account Has Been Activated Successfully");

                        if (seats != null)
                        {
                            seats.DeletedDate = DateTime.Now;
                            seats.DeletedBy = employeeId;
                            seatConfigurationModels.Add(seats);
                            _logger.LogInformation("Seat Status Has Been Changed To Unavailable");
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Employee Id not Found");
                        var responseDto = ResponseExtensions.ErrorResponse<string>(null, ResponseMessage.EmployeeIdNotFound, HttpStatusCode.BadRequest);
                        return responseDto;
                    }
                }
                else
                {
                    var userresult = await _repository.GetEmployee(user.EmployeeId);
                    if (userresult != null)
                    {
                        userresult.IsActive = user.IsActive;
                        userresult.DeletedDate = null; ;
                        userresult.DeletedBy = null;
                        userRegistrationModels.Add(userresult);
                    }
                    else
                    {
                        _logger.LogInformation("Employee Id not Found");
                        var responseDto = ResponseExtensions.ErrorResponse<string>(null, ResponseMessage.EmployeeIdNotFound, HttpStatusCode.BadRequest);
                        return responseDto;
                    }
                }
            }

            _logger.LogInformation("The Account Has Been InActivated Successfully");
            await _repository.UpdateUserStatus(userRegistrationModels);
            await _seatRepository.UpdateSeatStatus(seatConfigurationModels);
            var successResponse = new ResponseDto<string>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = ResponseMessage.SavedSuccessfully
            };
            return successResponse;
        }

        public async Task<ResponseDto<GetUserResultsDto>> GetEmployeeById(string employeeId)
        {
            var responseModel = await _repository.GetEmployeeById(employeeId);
            var employee = responseModel.FirstOrDefault();
            string base64String = null;

            if (employee.ProfilePictureFilePath != null)
            {
                byte[] imageBytes = File.ReadAllBytes(employee.ProfilePictureFilePath);
                base64String = Convert.ToBase64String(imageBytes);
            }

            var userResultsDto = new GetUserResultsDto
            {
                ProfilePictureFileString = base64String,
                EmailId = employee.EmailId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                PhoneNumber = employee.PhoneNumber
            };

            if (employee.DesignationId != null)
            {
                userResultsDto.designation = new UserCommonDto
                {
                    Id = employee.DesignationId,
                    Name = employee.DesignationName
                };
            }

            if (employee.ModeOfWorkId != null)
            {
                userResultsDto.modeOfWork = new UserCommonDto
                {
                    Id = employee.ModeOfWorkId,
                    Name = employee.ModeOfWork
                };
            }

            if (employee.CityId != null)
            {
                userResultsDto.city = new UserCommonDto
                {
                    Id = employee.CityId,
                    Name = employee.CityName
                };
            }

            if (employee.FloorId != null)
            {
                userResultsDto.floor = new UserCommonDto
                {
                    Name = employee.FloorName,
                    Id = employee.FloorId
                };
            }

            if (employee.ColumnId != null)
            {
                userResultsDto.column = new UserCommonDto
                {
                    Name = employee.ColumnName,
                    Id = employee.ColumnId
                };
            }

            if (employee.SeatId != null)
            {
                userResultsDto.seat = new GetSeatUserDto
                {
                    Id = employee.SeatId,
                    SeatNumber = employee.SeatNumber

                };
            }

            if (employee.WorkingDayId != null)
            {
                userResultsDto.days = responseModel
                    .Select(model => new GetUserDayDto { Day = model.WorkingDay, Id = model.WorkingDayId })
                    .ToList();

            }
            var response = new ResponseDto<GetUserResultsDto>()
            {
                Data = userResultsDto,
                StatusCode = 200
            };
            return response;
        }
    }
}







