using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using WebApi.Application.Common;
using WebApi.Application.Exceptions;

namespace WebApi.Api.Middlewares
{
    public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        private readonly RequestDelegate next = next;
        private readonly ILogger<ErrorHandlingMiddleware> logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (AppException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsJsonAsync(ApiResponse<object>.FailResponse(ex.Message, ex.Errors));
            }
            catch (FluentValidation.ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = ex.Errors
                                .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                                .ToList();

                await context.Response.WriteAsJsonAsync(ApiResponse<object>.FailResponse("Validation Error", errors));
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(ApiResponse<object>.FailResponse(ex.Message, [ex.Message]));
            }
            catch (SecurityTokenException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(ApiResponse<object>.FailResponse(ex.Message, [ex.Message]));
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;

                await context.Response.WriteAsJsonAsync(ApiResponse<object>.FailResponse(ex.Message, [ex.Message]));
            }
            catch (AccessForbiddenException ex)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                await context.Response.WriteAsJsonAsync(ApiResponse<object>.FailResponse(ex.Message, [ex.Message]));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled error");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(ApiResponse<object>.FailResponse("Internal server error", [ex.Message]));
            }
        }
    }
}