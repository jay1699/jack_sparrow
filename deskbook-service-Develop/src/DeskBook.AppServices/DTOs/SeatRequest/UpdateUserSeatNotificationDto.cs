namespace DeskBook.AppServices.DTOs.SeatRequest
{
    public class UpdateUserSeatNotificationDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime BookingDate { get; set; }

        public string Location { get; set; }

        public byte Floor { get; set; }

        public byte SeatNumber { get; set; }

        public byte RequestStatus { get; set; }

        public DateTime? ModifieDate { get; set; }

        public string? ModifiedBy { get; set; }
    }
}