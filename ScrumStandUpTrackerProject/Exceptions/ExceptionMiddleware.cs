using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ScrumStandUpTrackerProject.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext httpContext) //This is the method that ASP.NET Core runs for every request.
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message;

            // You can customize for different exceptions here
            switch (exception)
            {
                case KeyNotFoundException:  // 404 Not Found
                    status = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
                case ArgumentException:  // 400 Bad Request
                    status = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
                case UnauthorizedAccessException:
                    status = HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    break;
                default:  // 500 Internal Server Error
                    status = HttpStatusCode.InternalServerError;
                    message = "Internal Server Error.";
                    break;
            }

            var response = new { StatusCode = (int)status, Message = message };
            var payload = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(payload);
        }
    }
}
