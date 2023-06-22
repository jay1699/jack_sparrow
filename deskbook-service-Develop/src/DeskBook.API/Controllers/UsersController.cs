using DeskBook.AppServices.Contracts.UserRegistration;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.UserRegistration;
using DeskBook.AppServices.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DeskBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(CustomValidationExceptionFilter))]
    public class UsersController : BaseController
    {
        private readonly IUserRegistrationServices _userRegistrationServices;

        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRegistrationServices userRegistrationServices, ILogger<UsersController> logger)
        {
            _userRegistrationServices = userRegistrationServices;
            _logger = logger;
        }

        [SwaggerResponse(200, Type = typeof(ResponseDto<object>))]
        [SwaggerResponse(500, Type = typeof(ResponseDto<object>))]
        [SwaggerResponse(400, Type = typeof(ResponseDto<object>))]
        [HttpPost("register")]
        public async Task<IActionResult> AddUser([FromBody] UserRequestDto userRequestDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var key in ModelState.Keys)
                {
                    if (ModelState[key].ValidationState == ModelValidationState.Invalid)
                    {
                        var errorMessage = new List<string>();
                        var errorMessages = ModelState[key].Errors.Select(e => e.ErrorMessage).FirstOrDefault();
                        errors.Add(errorMessages);
                    }
                }
                _logger.LogError("Error in Validation of RequestDto");

                return BadRequest(new ResponseDto<object>
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Error = errors
                });
            }

            var result = await _userRegistrationServices.RegisterUserWithAuthority(userRequestDto);
            if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("Email Id already Exist");
                return BadRequest(result);
            }
            if (result != null && result.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                _logger.LogError("Error while Registerring User");
                return StatusCode(500, result);
            }
            _logger.LogInformation("User registration Successful");
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int pageNo, [FromQuery] int pageSize, [FromQuery] string search)
        {
            var result = await _userRegistrationServices.GetUsers(pageNo, pageSize, search);
            if (result != null && result.StatusCode == (int)HttpStatusCode.OK)
            {
                _logger.LogError("All Users are retrieved successfully.");
                return StatusCode(200, result);
            }
            else if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("Error while Getting Users");
                return BadRequest(result);
            }
            else
            {
                _logger.LogError("Internal Server Error");
                return StatusCode(500, result);
            }
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeById(string employeeId)
        {
            var result = await _userRegistrationServices.GetEmployeeById(employeeId);
            if (result != null && result.StatusCode == (int)HttpStatusCode.OK)
            {
                _logger.LogError("User is retrieved successfully.");
                return StatusCode(200, result);
            }
            else if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("Error while Getting Users");
                return BadRequest(result);
            }
            else
            {
                _logger.LogError("Internal Server Error");
                return StatusCode(500, result);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserStatus(List<UpdateUserRequestDto> userRequestDtos)
        {
            var result = await _userRegistrationServices.UpdateUserStatus(userRequestDtos, UserRegisteredId);
            if (result != null && result.StatusCode == (int)HttpStatusCode.OK)
            {
                _logger.LogError("Changes Have Made Successfully");
                return StatusCode(200, result);
            }
            else if (result != null && result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogError("EmployeeId not Found");
                return BadRequest(result);
            }
            else
            {
                _logger.LogError("Internal Server Error");
                return StatusCode(500, result);
            }
        }
    }
}