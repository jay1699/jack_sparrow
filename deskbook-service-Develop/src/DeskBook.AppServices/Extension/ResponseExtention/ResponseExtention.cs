using System.Net;
using DeskBook.AppServices.DTOs.Response;

namespace DeskBook.AppServices.Extension.ResponseExtention
{
    public static class ResponseExtensions
    {
        public static ResponseDto<T> ErrorResponse<T>(this T? data, string errorMessage, HttpStatusCode statusCode)
        {
            var errorDict = new List<string> { { errorMessage } };
            var responseDto = new ResponseDto<T>
            {
                StatusCode = (int)statusCode,
                Error = errorDict,
                Data = data
            };
            return responseDto;
        }
    }
}






