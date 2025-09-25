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
            object response = new { error = "An unexpected error occurred." };

            if (context.Exception is NotFoundException nf)
            {
                statusCode = 404;
                response = new { error = nf.Message };
            }
            else if (context.Exception is BusinessException be)
            {
                statusCode = 400;
                response = new
                {
                    message = "Validation failed",
                    errors = be.Errors   // هون صار يضيف كل الأخطاء
                };
            }

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
