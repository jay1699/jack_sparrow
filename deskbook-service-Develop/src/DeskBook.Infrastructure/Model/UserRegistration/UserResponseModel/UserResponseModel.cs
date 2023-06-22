namespace DeskBook.Infrastructure.Model.UserResponseModel
{
    public class UserResponseModel
    {
        public int StatusCode { get; set; }

        public List<string>? Error { get; set; }

        public dynamic? Responses { get; set; }
    }
}