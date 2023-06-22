namespace DeskBook.AppServices.DTOs.Seat
{
    public class UpdateSeatRequestDto
    {
        public int SeatId { get; set; }

        public bool IsAvailable { get; set; }

        public bool Unassigned { get; set; }
    }
}