namespace DeskBook.Infrastructure.Model.SeatRequest
{
    public class SeatBookingResponseModel
    {
        public string? Name { get; set; }

        public string? EmployeeId { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime AllottedDate { get; set; }

        public string? Email { get; set; }

        public int FloorNo { get; set; }

        public string? DeskNo { get; set; }
    }
}