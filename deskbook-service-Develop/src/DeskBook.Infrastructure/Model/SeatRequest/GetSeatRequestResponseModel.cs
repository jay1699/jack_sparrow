namespace DeskBook.Infrastructure.Model.SeatRequest
{
    public class GetSeatRequestResponseModel
    {
        public string? Name { get; set; }

        public string? EmployeeId { get; set; }

        public int SeatRequestId { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime RequestFor { get; set; }

        public string? Email { get; set; }

        public string? FloorNo { get; set; }

        public string? DeskNo { get; set; }

        public byte Status { get; set; }
    }
}