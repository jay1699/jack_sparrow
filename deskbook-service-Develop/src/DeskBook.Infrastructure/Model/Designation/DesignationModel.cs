using System.Text.Json.Serialization;
using DeskBook.Infrastructure.Model.UserRegistration;

namespace DeskBook.Infrastructure.Model.Designation
{
    public class DesignationModel
    {
        public byte DesignationId { get; set; }

        public string? DesignationName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }
        
        public UserRegistrationModel userRegistrationModel { get; set; }
    }
}