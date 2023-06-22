namespace DeskBook.AppServices.DTOs.RegisteredUser
{
    public class GetUserResultsDto
    {
        public string EmailId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ProfilePictureFileString { get; set; }

        public UserCommonDto designation { get; set; }

        public List<GetUserDayDto> days { get; set; }

        public UserCommonDto modeOfWork { get; set; }

        public UserCommonDto city { get; set; }

        public UserCommonDto column { get; set; }

        public UserCommonDto floor { get; set; }

        public GetSeatUserDto seat { get; set; }
    }
}