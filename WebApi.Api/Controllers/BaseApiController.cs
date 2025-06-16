using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Common;

namespace WebApi.Api.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {

        protected string? GetLoggedInUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        protected IActionResult Success<T>(T? data, string message = "Success")
        {
            return Ok(ApiResponse<T>.SuccessResponse(data, message));
        }

        protected IActionResult Fail<T>(string message, int statusCode = 400, List<string>? errors = null)
        {
            if (statusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(ApiResponse<T>.FailResponse(message, errors));
            }

            if (statusCode == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized(ApiResponse<T>.FailResponse(message, errors));
            }

            if (statusCode == StatusCodes.Status403Forbidden)
            {
                return Forbid();
            }

            if (statusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(ApiResponse<T>.FailResponse(message, errors));
            }

            if (statusCode == StatusCodes.Status401Unauthorized)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<T>.FailResponse(message, errors));
            }

            return StatusCode(statusCode: statusCode, value: ApiResponse<T>.FailResponse(message, errors));
        }
    }
}