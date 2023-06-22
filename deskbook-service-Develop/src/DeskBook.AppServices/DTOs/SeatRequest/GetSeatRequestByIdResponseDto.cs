namespace DeskBook.AppServices.DTOs.SeatRequest
{
    public class GetSeatRequestByIdResponseDto
    {
        public string RequestDate { get; set; }

        public string RequestedFor { get; set; }

        public string EmployeeId { get; set; }

        public string Name { get; set; }

        public int SeatId { get; set; }

        public string Email { get; set; }

        public string FloorNo { get; set; }

        public string DeskNo { get; set; }

        public string Reason { get; set; }
    }
}