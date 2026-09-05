using FluentValidation;
using OpenLedger.Application.Dtos;
using OpenLedger.Application.Exceptions;
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
                logger.LogWarning(ex, "An unhandled exception occurred while processing request {TraceId}", traceId);

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
                case BadRequestException badRequestException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errors.Add(badRequestException.Message);
                    break;
                case UnauthorizedException unauthorizedException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errors.Add(unauthorizedException.Message);
                    break;
                case NotFoundException notFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errors.Add(notFoundException.Message);
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
