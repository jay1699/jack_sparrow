using DeskBook.Infrastructure.Model.UserRegistration;

namespace DeskBook.Infrastructure.Contracts.UserRegistration
{
    public interface IUserRegistrationRepository
    {
        Task AddUser(UserRegistrationModel userRegistration);

        Task<string> GetByEmail(string email);

        Task<UserRegistrationModel> GetById(string employeeId);

        Task UpdateUserStatus(List<UserRegistrationModel> userRegistrationModels);

        Task<List<GetRegistredUserResponseModel>> GetUsers(int pageNo, int pageSize, string search);

        Task<UserRegistrationModel> GetEmployee(string employeeId);

        Task<List<GetUserResponseModel>> GetEmployeeById(string employeeId);
    }
}