namespace DeskBook.Infrastructure.Model.EmailModel
{
    public class EmailResponseModel
    {
        public string? EmployeeName { get; set; }

        public string? To { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }

        public string? BookingDate { get; set; }

        public string? OfficeLocation { get; set; }

        public string? FloorNumber { get; set; }

        public string? SeatNumber { get; set; }

        public string? Duration { get; set; }

    }
}