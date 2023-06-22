using System.Text.Json.Serialization;
using DeskBook.Infrastructure.Model.Seat;
using DeskBook.Infrastructure.Model.UserRegistration;

namespace DeskBook.Infrastructure.Model.SeatRequest
{
    public class SeatRequestModel
    {
        public int SeatRequestId { get; set; }

        public string EmployeeId { get; set; }

        public int SeatId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime BookingDate { get; set; }

        public string Reason { get; set; }

        public byte RequestStatus { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        [JsonIgnore]
        public UserRegistrationModel user { get; set; }

        [JsonIgnore]
        public UserRegistrationModel modifiedUser { get; set; }

        public SeatModel seat { get; set; }
    }
}