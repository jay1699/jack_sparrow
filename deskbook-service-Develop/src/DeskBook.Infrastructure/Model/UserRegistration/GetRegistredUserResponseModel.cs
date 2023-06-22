namespace DeskBook.Infrastructure.Model.UserRegistration
{

    public class GetRegistredUserResponseModel
    {
        public string EmployeeId { get; set; }

        public string Name { get; set; }

        public string EmailId { get; set; }

        public string? DesignationName { get; set; }

        public bool IsActive { get; set; }
    }
}
