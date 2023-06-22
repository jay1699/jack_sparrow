using DeskBook.Infrastructure.Model.Column;

namespace DeskBook.Infrastructure.Model.Seat
{
    public class SeatResponseModel
    {
        public int SeatId { get; set; }

        public byte SeatNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public byte ColumnId { get; set; }

        public string ColumnName { get; set; }

        public bool IsAvailable { get; set; }

        public ColumnModel ColumnModel { get; set; }

        public SeatConfigurationModel SeatConfigurationModel { get; set; }

        public string? EmployeeId { get; set; }

        public string ModeOfWork { get; set; }

        public string DesignationName { get; set; }

        public string Day { get; set; }

        public string SeatStatus { get; set; }
    }
}
