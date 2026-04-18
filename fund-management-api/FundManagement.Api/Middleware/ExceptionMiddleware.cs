using FundManagement.Api.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FundManagement.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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

        private static async Task HandleException(HttpContext context, Exception ex)
        {
            var problem = new ProblemDetails
            {
                Instance = context.Request.Path
            };

            switch (ex)
            {
                case NotFoundException:
                    problem.Title = "Resource not found";
                    problem.Status = StatusCodes.Status404NotFound;
                    problem.Detail = ex.Message;
                    break;

                case BadRequestException:
                    problem.Title = "Bad request";
                    problem.Status = StatusCodes.Status400BadRequest;
                    problem.Detail = ex.Message;
                    break;

                default:
                    problem.Title = "Internal Server Error";
                    problem.Status = StatusCodes.Status500InternalServerError;
                    problem.Detail = "Something went wrong";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = problem.Status.Value;

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
