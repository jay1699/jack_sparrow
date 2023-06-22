namespace DeskBook.AppServices.DTOs.UserRegistration
{
    public class UserRegistrationDto
    {
        public string UserClient { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public List<ClaimsDto> Claims { get; set; }

        public List<RolesDto> Roles { get; set; }
    }
}