using System.Text.Json.Serialization;
using DeskBook.Infrastructure.Model.Column;
using DeskBook.Infrastructure.Model.SeatRequest;
using DeskBook.Infrastructure.Model.UserRegistration;

namespace DeskBook.Infrastructure.Model.Seat
{
    public class SeatModel
    {
        public int SeatId { get; set; }

        public byte SeatNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public byte ColumnId { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        [JsonIgnore]
        public ColumnModel columnModel { get; set; }

        [JsonIgnore]
        public List<SeatConfigurationModel> seatConfigurationModel { get; set; }

        [JsonIgnore]
        public UserRegistrationModel user { get; set; }

        [JsonIgnore]
        public List<SeatRequestModel> seatRequests { get; set; }
    }
}


