namespace DeskBook.AppServices.Services.SeatRequest
{
    public class GetSeatRequestResultsDto
    {
        public string Name { get; set; }

        public string EmployeeId { get; set; }

        public int SeatRequestId { get; set; }

        public string RequestDate { get; set; }

        public string RequestFor { get; set; }

        public string Email { get; set; }

        public string FloorNo { get; set; }

        public string DeskNo { get; set; }

        public byte Status { get; set; }
    }
}