using DeskBook.Infrastructure.Model.UserRegistration;

namespace DeskBook.Infrastructure.Model.Seat
{
    public class EmployeeWorkingDaysModel
    {
        public int EmployeeWorkingDayId { get; set; }

        public string EmployeeId { get; set; }

        public byte WorkingDayId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string? DeletedBy { get; set; }

        public UserRegistrationModel userRegistrationModel { get; set; }
    }
}