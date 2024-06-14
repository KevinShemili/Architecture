using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IHostEnvironment _environment;
        // inject Log

        public GlobalExceptionHandler(IHostEnvironment environment)
        {
            _environment = environment;
        }


        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, 
            CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails();

            // Include stack trace only in development environment
            if (_environment.IsDevelopment() || _environment.EnvironmentName == "Docker")
            {
                problemDetails.Extensions["StackTrace"] = exception.StackTrace;
                problemDetails.Detail = exception.Message;
            }

            switch (exception)
            {
                case ValidationException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Validation Failure";
                    // Log the validation error here
                    break;                

                case UnauthorizedAccessException:
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    problemDetails.Title = "Unauthorized Access";
                    // Log the unauthorized access error here
                    break;

                default:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Internal Server Error";
                    // Log the internal server error here
                    break;
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
