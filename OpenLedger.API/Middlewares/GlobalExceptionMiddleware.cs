using OpenLedger.Application.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;

namespace OpenLedger.API.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try {
                await next(context);
            }
            catch (Exception ex) {
                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                logger.LogError(ex, "An unhandled exception occurred while processing the request at {TraceId}, {Timestamp}", traceId, DateTime.UtcNow);
                await HandleExceptionAsync(context, ex, traceId);
            }
        }
        public async Task HandleExceptionAsync(HttpContext context, Exception exception, string traceId)
        {
            switch (exception)
            {
                case ArgumentNullException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case ValidationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";

            var ErrorResponse = new ErrorResponseDto(
                StatusCode: context.Response.StatusCode,
                Message:  exception.Message,
                Exception: exception.ToString(),
                Errors: [exception.StackTrace ?? string.Empty],
                TraceId: traceId
            );
            await context.Response.WriteAsJsonAsync(ErrorResponse);
        }
    }
}
