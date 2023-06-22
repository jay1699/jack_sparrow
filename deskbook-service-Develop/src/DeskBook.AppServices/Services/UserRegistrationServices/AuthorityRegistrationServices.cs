using DeskBook.AppServices.Contracts.AuthorityRegistration;
using DeskBook.AppServices.DTOs.Response;
using DeskBook.AppServices.DTOs.UserRegistration;
using DeskBook.Infrastructure.Contracts.ITokenRepository;
using DeskBook.Infrastructure.Contracts.RestServiceClient;
using DeskBook.Infrastructure.Model.AuthoritySetting;
using Microsoft.Extensions.Logging;

namespace DeskBook.AppServices.Services.UserRegistrationServices
{
    public class AuthorityRegistrationServices : IAuthorityRegistrationServices
    {
        private readonly IRestServiceClientRepository _restserviceClient;

        private readonly ITokenRepository _tokenrepository;

        private readonly AuthorityModel _authoritySettings;

        private readonly ILogger<AuthorityRegistrationServices> _logger;

        public AuthorityRegistrationServices(ITokenRepository tokenrepository, IRestServiceClientRepository restserviceClient, AuthorityModel authoritySettings, ILogger<AuthorityRegistrationServices> logger)
        {
            _tokenrepository = tokenrepository;
            _restserviceClient = restserviceClient;
            _authoritySettings = authoritySettings;
            _logger = logger;
        }

        public virtual async Task<ResponseDto<UserResponse>> RegisterUser(UserRegistrationDto authorityRegisterModel)
        {
            string baseUri = _authoritySettings.BaseUri;

            var token = await _tokenrepository.GenerateTokenAsync();

            var response = await _restserviceClient.InvokePostAsync<UserRegistrationDto, ResponseDto<UserResponse>>(baseUri, authorityRegisterModel, token);

            var Response = new ResponseDto<UserResponse>()
            {
                Data = new UserResponse()
                {
                    StatusCode = response.StatusCode,
                    Responses = response.Responses
                },
                StatusCode = response.StatusCode,
                Error = response.Error
            };
            return Response;
        }
    }
}

