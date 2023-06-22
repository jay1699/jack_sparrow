using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.UserRegistration;

namespace DeskBook.AppServices.Contracts.AuthorityRegistration
{
    public interface IAuthorityRegistrationServices
    {
        Task<ResponseDto<UserResponse>> RegisterUser(UserRegistrationDto authorityRegisterModel);
    }
}