using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Api.Wrappers;

namespace Api.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            int statusCode = 500;
            ApiResponse response = ApiResponse.Response(500, "An unexpected error occurred.");

            if (context.Exception is NotFoundException nf)
            {
                statusCode = 404;
                response = ApiResponse.Response(404, nf.Message);
            }
            else if (context.Exception is BusinessException be)
            {
                statusCode = 400;
                response = ApiResponse.Response(400, "Validation failed", be.Errors);
            }

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
