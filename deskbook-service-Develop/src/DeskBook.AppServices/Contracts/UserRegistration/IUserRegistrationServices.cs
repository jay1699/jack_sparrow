using DeskBook.AppServices.DTOs.RegisteredUser;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.UserRegistration;

namespace DeskBook.AppServices.Contracts.UserRegistration
{
    public interface IUserRegistrationServices
    {
        Task<ResponseDto<string>> RegisterUserWithAuthority(UserRequestDto userRequest);

        Task<ResponseDto<string>> UpdateUserStatus(List<UpdateUserRequestDto> userStatusDto, string employeeId);

        Task<ResponseDto<List<GetRegisteredUserResultDto>>> GetUsers(int pageNo, int pageSize, string search);

        Task<ResponseDto<GetUserResultsDto>> GetEmployeeById(string employeeId);
    }
}