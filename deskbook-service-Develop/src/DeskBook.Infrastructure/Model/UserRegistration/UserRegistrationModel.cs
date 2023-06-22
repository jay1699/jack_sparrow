using System.Text.Json.Serialization;
using DeskBook.Infrastructure.Model.Designation;
using DeskBook.Infrastructure.Model.Seat;
using DeskBook.Infrastructure.Model.SeatRequest;

namespace DeskBook.Infrastructure.Model.UserRegistration
{
    public class UserRegistrationModel
    {
        public string EmployeeId { get; set; }

        public string EmailId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public byte? ModeOfWorkId { get; set; }

        public string? ProfilePictureFileName { get; set; }

        public string? ProfilePictureFilePath { get; set; }

        public byte? DesignationId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public bool IsActive { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string? DeletedBy { get; set; }

        public SeatConfigurationModel seatConfigurationModel { get; set; }

        [JsonIgnore]
        public EmployeeWorkingDaysModel employeeWorkingDays { get; set; }

        [JsonIgnore]
        public DesignationModel designationModel { get; set; }

        [JsonIgnore]
        public List<SeatModel> seats { get; set; }

        [JsonIgnore]
        public List<SeatRequestModel> seatRequests { get; set; }
    }
}
