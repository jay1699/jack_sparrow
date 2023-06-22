namespace DeskBook.AppServices.DTOs.UserRegistration
{
    public class UpdateUserRequestDto
    {
        public bool IsActive { get; set; }

        public DateTime DeletedDate { get; set; }

        public string EmployeeId { get; set; }
    }
}