using DeskBook.Infrastructure.Model.UserResponseModel;

namespace DeskBook.Infrastructure.Contracts.RestServiceClient
{
    public interface IRestServiceClientRepository
    {
        Task<UserResponseModel> InvokePostAsync<T, R>(string requestUri, T model, string token = null) where R : new();
    }
}