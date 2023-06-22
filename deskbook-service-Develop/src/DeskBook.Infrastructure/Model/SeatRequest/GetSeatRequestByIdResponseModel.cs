namespace DeskBook.Infrastructure.Model.SeatRequest
{
    public class GetSeatRequestByIdResponseModel
    {
        public DateTime RequestDate { get; set; }

        public DateTime RequestedFor { get; set; }

        public string? EmployeeId { get; set; }

        public string? Name { get; set; }

        public int SeatId { get; set; }

        public string? Email { get; set; }

        public string? City { get; set; }

        public string? FloorNo { get; set; }

        public string? SeatNumber { get; set; }

        public string? DeskNo { get; set; }

        public string? Reason { get; set; }
    }
}