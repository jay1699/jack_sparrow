namespace DeskBook.Infrastructure.Model.Seat
{
    public class ModeOfWorkModel
    {
        public byte ModeOfWorkId { get; set; }

        public string ModeOfWork { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }
    }
}