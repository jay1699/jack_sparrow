namespace DeskBook.AppServices.DTOs.RegisteredUser
{
    public class GetRegisteredUserResultDto
    {
        public string EmployeeId { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string? Designation { get; set; }

        public bool Status { get; set; }
    }
}