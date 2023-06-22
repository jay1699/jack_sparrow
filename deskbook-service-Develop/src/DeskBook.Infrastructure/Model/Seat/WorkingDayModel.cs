namespace DeskBook.Infrastructure.Model.Seat
{
    public class WorkingDayModel
    {
        public byte WorkingDayId { get; set; }

        public string Day { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }
    }
}