using FundManagement.Api.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FundManagement.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case NotFoundException:
                {
                    var problem = new ProblemDetails
                    {
                        Title = "Resource not found",
                        Status = StatusCodes.Status404NotFound,
                        Detail = ex.Message,
                        Instance = context.Request.Path
                    };
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsJsonAsync(problem);
                    break;
                }
                case BadRequestException:
                {
                    var problem = new ProblemDetails
                    {
                        Title = "Bad request",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = ex.Message,
                        Instance = context.Request.Path
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(problem);
                    break;
                }
                default:
                {
                    _logger.LogError(ex, "Unhandled exception");
                    var problem = new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "Something went wrong",
                        Instance = context.Request.Path
                    };
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(problem);
                    break;
                }
            }
        }
    }
}
