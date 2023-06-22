namespace DeskBook.Infrastructure.Model.SeatRequest
{
    public class GetUserSeatRequestResponseModel
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public DateTime BookingDate { get; set; }

        public int SeatRequestId { get; set; }

        public string? Location { get; set; }

        public string? Floor { get; set; }

        public string? SeatNumber { get; set; }

        public byte RequestStatus { get; set; }

        public DateTime? ModifieDate { get; set; }

        public string? ModifiedBy { get; set; }
    }
}