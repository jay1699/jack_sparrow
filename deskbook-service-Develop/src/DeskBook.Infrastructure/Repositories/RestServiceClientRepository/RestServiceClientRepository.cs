using DeskBook.Infrastructure.Contracts.RestServiceClient;
using DeskBook.Infrastructure.Model.UserResponseModel;
using Microsoft.Extensions.Logging;
using RestSharp;


namespace DeskBook.Infrastructure.Repositories.RestServiceClientRepository
{
    public class RestServiceClientRepository : IRestServiceClientRepository
    {

        private readonly ILogger<RestServiceClientRepository> _logger;

        public RestServiceClientRepository(ILogger<RestServiceClientRepository> logger)
        {
            _logger = logger;
        }

        public async Task<UserResponseModel> InvokePostAsync<T, R>(string requestUri, T model, string? token = null) where R : new()
        {
            var client = new RestSharp.RestClient(requestUri);

            var restRequest = new RestSharp.RestRequest(requestUri, Method.POST)
            {
                OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; }
            };

            restRequest.AddJsonBody(model);

            if (!string.IsNullOrWhiteSpace(token))
            {
                restRequest.AddParameter("Authorization", token,
                ParameterType.HttpHeader);
            }

            IRestResponse<UserResponseModel> response = client.Execute<UserResponseModel>(restRequest);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("POST request to {requestUri} succeeded with response code {statusCode} and response content {responseContent}",
                                     requestUri, response.StatusCode, response.Content);
                return new UserResponseModel
                {
                    StatusCode = 200,
                    Responses = response.Content
                };
            }
            else
            {
                _logger.LogInformation("POST request to {requestUri} failed with response code {statusCode}.",
                                    requestUri, response.StatusCode, response.Content);
                var error = new List<string>();
                error.Add("User Registration Failed");
                return new UserResponseModel
                {
                    StatusCode = (int)response.StatusCode,
                    Error = error
                };
            }
        }
    }
}
