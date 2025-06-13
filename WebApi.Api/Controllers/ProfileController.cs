using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.DTOs.Request.Auth;
using WebApi.Application.Interfaces.Service;

namespace WebApi.Api.Controllers
{
    [ApiController]
    [Route("api/v1/profile")]
    public class ProfileController(IAuthService authService) : BaseApiController
    {

        private readonly IAuthService authService = authService;

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await authService.GetUserProfileAsync(userId);

            return Success(result);
        }

        [Authorize]
        [HttpPost("")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await authService.UpdateProfileAsync(userId, request);

            return Success(result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await authService.ChangePasswordAsync(userId, request);

            return Success<object>(null, "Change password success");
        }

    }
}