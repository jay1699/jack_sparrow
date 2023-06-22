namespace DeskBook.AppServices.DTOs.Seat
{
    public class GetSeatResponseDto
    {
        public List<GetSeatResultsDto> bookedSeat { get; set; }

        public List<GetSeatResultsDto> availableforBookingSeat { get; set; }

        public List<GetSeatResultsDto> reservedSeat { get; set; }

        public List<GetSeatResultsDto> unavailableSeat { get; set; }

        public List<GetSeatResultsDto> unallocatedSeat { get; set; }
    }
}