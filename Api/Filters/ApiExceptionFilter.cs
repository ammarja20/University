using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            int statusCode = 500;
            string message = "An unexpected error occurred.";

            if (context.Exception is NotFoundException nf)
            {
                statusCode = 404;
                message = nf.Message;
            }
            else if (context.Exception is BusinessException be)
            {
                statusCode = 400;
                message = be.Message;
            }

            context.Result = new ObjectResult(new { error = message })
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
