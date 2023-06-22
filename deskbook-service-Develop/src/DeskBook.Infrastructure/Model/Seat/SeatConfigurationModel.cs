using System.Text.Json.Serialization;
using DeskBook.Infrastructure.Model.UserRegistration;

namespace DeskBook.Infrastructure.Model.Seat
{
public class SeatConfigurationModel
{
    public int SeatConfigurationId { get; set; }

    public int SeatId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public string EmployeeId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public string? DeletedBy { get; set; }

    [JsonIgnore]
    public UserRegistrationModel userRegistrationModel { get; set; }
    
    [JsonIgnore]    
    public SeatModel seatModels { get; set; }
}
}