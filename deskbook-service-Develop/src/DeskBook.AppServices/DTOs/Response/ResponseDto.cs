namespace DeskBook.AppServices.DTOs.Response
{
    public class ResponseDto<T>
    {
        public T Data { get; set; }

        public List<string> Error { get; set; }

        public int StatusCode { get; set; }
    }
}