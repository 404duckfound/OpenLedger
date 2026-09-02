using FluentValidation;
using OpenLedger.Application.Dtos;
using System.Diagnostics;
using System.Net;

namespace OpenLedger.API.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                logger.LogError(ex, "An unhandled exception occurred while processing the request at {TraceId}, {Timestamp}, {Message}", traceId, DateTime.UtcNow, ex.Message);

                await HandleExceptionAsync(context, ex, traceId);
            }
        }
        public async Task HandleExceptionAsync(HttpContext context, Exception exception, string traceId)
        {            
            var errors = new List<string>();

            switch (exception)
            {
                case ValidationException validationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    foreach (var error in validationException.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                    break;
                case ArgumentNullException argumentNullException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errors.Add(argumentNullException.Message);
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errors.Add(unauthorizedAccessException.Message);
                    break;
                case InvalidOperationException invalidOperationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errors.Add(invalidOperationException.Message);
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errors.Add(env.IsDevelopment() ? exception.ToString() : "An unexpected error occurred.");
                break;
            }

            var ErrorResponse = new ErrorResponseDto(
                StatusCode: context.Response.StatusCode,
                ExceptionType: exception.GetType().Name,
                Errors: errors,
                TraceId: traceId
            );

            await context.Response.WriteAsJsonAsync(ErrorResponse);
        }
    }
}
