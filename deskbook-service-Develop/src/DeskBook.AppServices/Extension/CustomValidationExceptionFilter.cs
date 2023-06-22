using System.Net;
using DeskBook.AppServices.DTOs.Response;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeskBook.AppServices.Extension
{
    public class CustomValidationExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                var errorResponse = new ResponseDto<string>
                {
                    Error = new List<string>(),
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

                foreach (var error in validationException.Errors)
                {
                    errorResponse.Error.Add(error.ErrorMessage);
                }
                context.Result = new JsonResult(errorResponse);
            }
        }
    }
}