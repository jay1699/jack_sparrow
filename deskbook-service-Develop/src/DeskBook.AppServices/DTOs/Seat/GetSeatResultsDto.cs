namespace DeskBook.AppServices.DTOs.Seat
{
    public class GetSeatResultsDto : SeatRequestDto
    {
        public string ColumnName { get; set; }

        public int SeatId { get; set; }
    }
}
