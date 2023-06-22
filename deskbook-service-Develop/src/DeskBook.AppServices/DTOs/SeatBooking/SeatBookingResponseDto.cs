namespace DeskBook.AppServices.DTOs.SeatBooking
{
    public class SeatBookingResponseDto
    {
        public string Name { get; set; }

        public string RequestDate { get; set; }

        public string AllottedDate { get; set; }

        public string Email { get; set; }

        public int FloorNo { get; set; }

        public string DeskNo { get; set; }
    }
}