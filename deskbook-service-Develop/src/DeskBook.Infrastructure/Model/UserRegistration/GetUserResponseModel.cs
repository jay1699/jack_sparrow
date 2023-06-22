namespace DeskBook.Infrastructure.Model.UserRegistration
{
    public class GetUserResponseModel
    {
        public string? EmployeeId { get; set; }

        public string? EmailId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ModeOfWork { get; set; }

        public string? ProfilePictureFileName { get; set; }

        public string? ProfilePictureFilePath { get; set; }

        public string? DesignationName { get; set; }

        public bool IsActive { get; set; }

        public string? CityName { get; set; }

        public string? FloorName { get; set; }

        public string? ColumnName { get; set; }

        public byte? SeatNumber { get; set; }

        public string? WorkingDay { get; set; }

        public int? CityId { get; set; }

        public byte? FloorId { get; set; }

        public byte? ColumnId { get; set; }

        public byte? WorkingDayId { get; set; }

        public int? SeatId { get; set; }

        public byte? DesignationId { get; set; }

        public byte? ModeOfWorkId { get; set; }
    }
}